---
title: How-to: Use the HL7 Results Gateway API (Lab & Developer)
date_created: 2025-09-01
last_updated: 2025-09-01
audience: [Laboratory Staff, Software Developers]
status: Draft
---

## How-to: Use the HL7 Results Gateway API


## Introduction

- Purpose of this guide
- Who should use this guide (lab staff, developers)
- Prerequisites (API access, .http file support, Azure region: UK South)


## 1. Authenticate to the API

- Overview of authentication method (e.g., API key, Azure AD, etc.)
- Step-by-step authentication instructions
- Example authentication request (.http file snippet)
- Troubleshooting authentication issues


## 2. Submit an HL7 Message

- HL7 message format requirements
- How to use the `/api/hl7/process` endpoint
- Example .http file for message submission
- Expected responses and error handling


## 3. Check API Health

- Using the `/api/health` endpoint
- Example .http file for health check
- Interpreting health check results


## 4. Validate and Interpret Results

- Understanding API responses
- How to verify successful message processing
- Common error scenarios and solutions


## 5. Testing with .http Files

- How to use provided .http files for local and remote testing
- Editing and running .http files in VS Code or other tools
- Example test workflow


## 6. Troubleshooting & FAQ

- Common issues and solutions
- Where to get help or report problems


## 7. Further Reading & References

- Links to API reference, Diátaxis docs, project README, Azure Functions docs

---

**Note:** All examples use the UK South Azure region. Do not include real credentials or sensitive data in requests.
