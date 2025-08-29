using HL7ResultsGateway.Domain.Entities;
using HL7ResultsGateway.Domain.Exceptions;
using HL7ResultsGateway.Domain.ValueObjects;

namespace HL7ResultsGateway.Domain.Services;

public class HL7MessageParser : IHL7MessageParser
{
    private const string MSH_SEGMENT = "MSH";
    private const string PID_SEGMENT = "PID";
    private const string OBX_SEGMENT = "OBX";
    private const char FIELD_SEPARATOR = '|';
    private const char COMPONENT_SEPARATOR = '^';

    public HL7Result ParseMessage(string message)
    {
        ValidateMessage(message);

        var lines = NormalizeMessageLines(message);
        ValidateHL7Format(lines);

        var result = new HL7Result();

        foreach (var line in lines)
        {
            ParseSegment(line, result);
        }

        return result;
    }

    private static void ValidateMessage(string message)
    {
        if (string.IsNullOrEmpty(message))
        {
            throw new ArgumentException("HL7 message cannot be null or empty", nameof(message));
        }
    }

    private static string[] NormalizeMessageLines(string message)
    {
        return message.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                     .Select(line => line.Trim())
                     .Where(line => !string.IsNullOrEmpty(line))
                     .ToArray();
    }

    private static void ValidateHL7Format(string[] lines)
    {
        if (lines.Length == 0 || !lines[0].StartsWith(MSH_SEGMENT))
        {
            throw new HL7ParseException("Invalid HL7 message format - must start with MSH segment");
        }
    }

    private void ParseSegment(string line, HL7Result result)
    {
        var segments = line.Split(FIELD_SEPARATOR);
        var segmentType = segments[0];

        switch (segmentType)
        {
            case MSH_SEGMENT:
                ParseMSHSegment(segments, result);
                break;
            case PID_SEGMENT:
                ParsePIDSegment(segments, result);
                break;
            case OBX_SEGMENT:
                ParseOBXSegment(segments, result);
                break;
        }
    }

    private static void ParseMSHSegment(string[] segments, HL7Result result)
    {
        if (segments.Length > 8 && segments[8].Contains("ORU^R01"))
        {
            result.MessageType = HL7MessageType.ORU_R01;
        }
    }

    private static void ParsePIDSegment(string[] segments, HL7Result result)
    {
        if (segments.Length <= 3) return;

        result.Patient.PatientId = segments[3];
        ParsePatientName(segments, result.Patient);
        ParseDateOfBirth(segments, result.Patient);
        ParseGender(segments, result.Patient);
    }

    private static void ParsePatientName(string[] segments, Patient patient)
    {
        if (segments.Length <= 5 || string.IsNullOrEmpty(segments[5])) return;

        var nameParts = segments[5].Split(COMPONENT_SEPARATOR);
        if (nameParts.Length > 0) patient.LastName = nameParts[0];
        if (nameParts.Length > 1) patient.FirstName = nameParts[1];
        if (nameParts.Length > 2) patient.MiddleName = nameParts[2];
    }

    private static void ParseDateOfBirth(string[] segments, Patient patient)
    {
        if (segments.Length > 7 &&
            !string.IsNullOrEmpty(segments[7]) &&
            DateTime.TryParseExact(segments[7], "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out var dob))
        {
            patient.DateOfBirth = dob;
        }
    }

    private static void ParseGender(string[] segments, Patient patient)
    {
        if (segments.Length > 8 && !string.IsNullOrEmpty(segments[8]))
        {
            patient.Gender = segments[8].ToUpper() switch
            {
                "M" => Gender.Male,
                "F" => Gender.Female,
                _ => Gender.Unknown
            };
        }
    }

    private static void ParseOBXSegment(string[] segments, HL7Result result)
    {
        if (segments.Length <= 6) return;

        var observation = new Observation();

        PopulateObservationFields(segments, observation);
        result.Observations.Add(observation);
    }

    private static void PopulateObservationFields(string[] segments, Observation observation)
    {
        if (segments.Length > 2) observation.ValueType = segments[2];

        ParseObservationIdentifier(segments, observation);

        if (segments.Length > 5) observation.Value = segments[5];
        if (segments.Length > 6) observation.Units = segments[6];
        if (segments.Length > 7) observation.ReferenceRange = segments[7];

        ParseObservationStatus(segments, observation);
    }

    private static void ParseObservationIdentifier(string[] segments, Observation observation)
    {
        if (segments.Length <= 3 || string.IsNullOrEmpty(segments[3])) return;

        var observationParts = segments[3].Split(COMPONENT_SEPARATOR);
        if (observationParts.Length > 0) observation.ObservationId = observationParts[0];
        if (observationParts.Length > 1) observation.Description = observationParts[1];
    }

    private static void ParseObservationStatus(string[] segments, Observation observation)
    {
        if (segments.Length <= 8) return;

        observation.Status = segments[8].ToUpper() switch
        {
            "N" => ObservationStatus.Normal,
            "A" => ObservationStatus.Abnormal,
            "H" => ObservationStatus.Abnormal,
            "L" => ObservationStatus.Abnormal,
            "C" => ObservationStatus.Critical,
            _ => ObservationStatus.Unknown
        };
    }
}