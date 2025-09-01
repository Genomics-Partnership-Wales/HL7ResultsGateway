---
goal: "How-to Guide for Lab and Developer API Usage"
version: 1.0
date_created: 2025-09-01
last_updated: 2025-09-01
owner: "HL7 Results Gateway Documentation Team"
status: 'Planned'
tags: [feature, docs, howto, diataxis, api, onboarding]
---

# Introduction

![Status: Planned](https://img.shields.io/badge/status-Planned-blue)

This plan defines the implementation of a Diátaxis-compliant "How-to" guide for laboratory and developer users to interact with the HL7 Results Gateway API. The guide will provide step-by-step instructions for authenticating, submitting HL7 messages, and validating results using the API, with examples and troubleshooting.

## 1. Requirements & Constraints

- **REQ-001**: Guide must follow the Diátaxis "How-to" documentation pattern.
- **REQ-002**: Must be accessible to both laboratory staff and software developers.
- **REQ-003**: Must include authentication, message submission, and result validation steps.
- **REQ-004**: All examples must use the UK South Azure region.
- **REQ-005**: Must include .http file usage for API testing.
- **SEC-001**: No sensitive data or credentials in documentation.
- **CON-001**: Documentation must be Markdown, stored in `/docs/` and `/plan/`.
- **GUD-001**: Use plain language, clear steps, and code samples.
- **PAT-001**: Reference Diátaxis and project documentation standards.

## 2. Implementation Steps

### Implementation Phase 1

- GOAL-001: Define and approve the structure and outline of the how-to guide.

| Task | Description | Completed | Date |
|------|-------------|-----------|------|
| TASK-001 | Draft outline for the how-to guide, including all required sections | ✅ | 2025-09-01 |
| TASK-002 | Review and approve outline with project owner | ✅ | 2025-09-01 |
| TASK-003 | Identify all required code samples and .http files | ✅ | 2025-09-01 |

### Implementation Phase 2

- GOAL-002: Write, validate, and publish the how-to guide.

| Task | Description | Completed | Date |
|------|-------------|-----------|------|
| TASK-004 | Write the full how-to guide in `/docs/howto-api-usage.md` | ✅ | 2025-09-01 |
| TASK-005 | Add code samples and .http file usage examples | ✅ | 2025-09-01 |
| TASK-006 | Validate guide with lab and developer users (usability test) | ✅ | 2025-09-01 |
| TASK-007 | Review for security, accuracy, and accessibility | ✅ | 2025-09-01 |
| TASK-008 | Publish guide and update `/plan/feature-howto-api-usage-1.md` status |  |  |

## 3. Alternatives

- **ALT-001**: Integrate how-to content into a general README (rejected: not Diátaxis-compliant, less discoverable).
- **ALT-002**: Provide only API reference (rejected: lacks step-by-step guidance for new users).

## 4. Dependencies

- **DEP-001**: Existing API endpoints and .http files.
- **DEP-002**: Diátaxis documentation prompt and project documentation standards.
- **DEP-003**: Access to lab and developer user feedback.

## 5. Files

- **FILE-001**: `/docs/howto-api-usage.md` (main how-to guide)
- **FILE-002**: `/plan/feature-howto-api-usage-1.md` (this plan)
 - **FILE-003**: `/docs/howto-api-usage-outline.md` (approved outline)
 - **FILE-004**: `/src/HL7ResultsGateway.API/health-check.http` (.http file for health check)
 - **FILE-005**: `/src/HL7ResultsGateway.API/process-hl7-message.http` (.http file for HL7 message submission)
 - **FILE-006**: (to be created) Example .http file for authentication (if required by API)
 - **FILE-007**: (to be created) Example .http file for result validation (if not covered above)

## 6. Testing

- **TEST-001**: Usability test with at least one lab user and one developer.
- **TEST-002**: Automated check for code sample accuracy and .http file validity.
- **TEST-003**: Security review for sensitive data exposure.

## 7. Risks & Assumptions

- **RISK-001**: Users may have varying technical backgrounds; guide must be accessible.
- **RISK-002**: API endpoints or authentication may change, requiring guide updates.
- **ASSUMPTION-001**: All referenced API endpoints and .http files are up to date.

## 8. Related Specifications / Further Reading

- [Diátaxis Documentation Framework](https://diataxis.fr/)
- [HL7 Results Gateway Copilot Instructions](../.github/copilot-instructions.md)
- [Azure Functions API Reference](https://learn.microsoft.com/en-us/azure/azure-functions/)
- [Project README](../README.md)
