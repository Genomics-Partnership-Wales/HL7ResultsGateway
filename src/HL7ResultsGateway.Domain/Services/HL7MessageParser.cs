using HL7ResultsGateway.Domain.Entities;
using HL7ResultsGateway.Domain.Exceptions;
using HL7ResultsGateway.Domain.ValueObjects;

namespace HL7ResultsGateway.Domain.Services;

public class HL7MessageParser : IHL7MessageParser
{
    public HL7Result ParseMessage(string message)
    {
        // Handle empty/null message
        if (string.IsNullOrEmpty(message))
        {
            throw new ArgumentException("HL7 message cannot be null or empty", nameof(message));
        }

        // Clean up the message (remove extra whitespace)
        var cleanMessage = message.Replace("\r", "").Replace("\n", "");
        var lines = cleanMessage.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                                .Select(line => line.Trim())
                                .Where(line => !string.IsNullOrEmpty(line))
                                .ToArray();

        // Basic validation - must start with MSH
        if (lines.Length == 0 || !lines[0].StartsWith("MSH"))
        {
            throw new HL7ParseException("Invalid HL7 message format - must start with MSH segment");
        }

        var result = new HL7Result();

        foreach (var line in lines)
        {
            var segments = line.Split('|');
            var segmentType = segments[0];

            switch (segmentType)
            {
                case "MSH":
                    ParseMSHSegment(segments, result);
                    break;
                case "PID":
                    ParsePIDSegment(segments, result);
                    break;
                case "OBX":
                    ParseOBXSegment(segments, result);
                    break;
                    // Ignore OBR for now in this minimal implementation
            }
        }

        return result;
    }

    private void ParseMSHSegment(string[] segments, HL7Result result)
    {
        // MSH segment format: MSH|^~\&|SendingApp|SendingFacility|ReceivingApp|ReceivingFacility|Timestamp||MessageType|MessageControlId|ProcessingId|Version
        if (segments.Length > 8)
        {
            var messageType = segments[8]; // ORU^R01
            if (messageType.Contains("ORU^R01"))
            {
                result.MessageType = HL7MessageType.ORU_R01;
            }
        }
    }

    private void ParsePIDSegment(string[] segments, HL7Result result)
    {
        // PID segment format: PID|SetId||PatientId||Name||DOB|Gender|||Address
        if (segments.Length > 5)
        {
            result.Patient.PatientId = segments[3]; // PatientId

            // Parse name (format: LAST^FIRST^MIDDLE)
            if (segments.Length > 5 && !string.IsNullOrEmpty(segments[5]))
            {
                var nameParts = segments[5].Split('^');
                if (nameParts.Length > 0) result.Patient.LastName = nameParts[0];
                if (nameParts.Length > 1) result.Patient.FirstName = nameParts[1];
                if (nameParts.Length > 2) result.Patient.MiddleName = nameParts[2];
            }

            // Parse DOB (format: YYYYMMDD)
            if (segments.Length > 7 && !string.IsNullOrEmpty(segments[7]))
            {
                if (DateTime.TryParseExact(segments[7], "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out var dob))
                {
                    result.Patient.DateOfBirth = dob;
                }
            }

            // Parse Gender
            if (segments.Length > 8 && !string.IsNullOrEmpty(segments[8]))
            {
                result.Patient.Gender = segments[8].ToUpper() switch
                {
                    "M" => Gender.Male,
                    "F" => Gender.Female,
                    _ => Gender.Unknown
                };
            }
        }
    }

    private void ParseOBXSegment(string[] segments, HL7Result result)
    {
        // OBX segment format: OBX|SetId|ValueType|ObservationId^Description|SubId|Value|Units|ReferenceRange|AbnormalFlags|Probability|NatureOfAbnormalTest|ObservationResultStatus
        if (segments.Length > 6)
        {
            var observation = new Observation();

            // ValueType
            if (segments.Length > 2) observation.ValueType = segments[2];

            // ObservationId and Description (format: CODE^DESCRIPTION)
            if (segments.Length > 3 && !string.IsNullOrEmpty(segments[3]))
            {
                var observationParts = segments[3].Split('^');
                if (observationParts.Length > 0) observation.ObservationId = observationParts[0];
                if (observationParts.Length > 1) observation.Description = observationParts[1];
            }

            // Value
            if (segments.Length > 5) observation.Value = segments[5];

            // Units
            if (segments.Length > 6) observation.Units = segments[6];

            // Reference Range
            if (segments.Length > 7) observation.ReferenceRange = segments[7];

            // Abnormal Flags (N = Normal, A = Abnormal, etc.)
            if (segments.Length > 8)
            {
                observation.Status = segments[8].ToUpper() switch
                {
                    "N" => ObservationStatus.Normal,
                    "A" => ObservationStatus.Abnormal,
                    "H" => ObservationStatus.Abnormal, // High
                    "L" => ObservationStatus.Abnormal, // Low
                    "C" => ObservationStatus.Critical,
                    _ => ObservationStatus.Unknown
                };
            }

            result.Observations.Add(observation);
        }
    }
}