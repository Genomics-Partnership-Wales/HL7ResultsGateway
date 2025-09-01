---
title: How-to: Use the HL7 Results Gateway API (Lab & Developer)
date_created: 2025-09-01
last_updated: 2025-09-01
audience: [Laboratory Staff, Software Developers]
status: Draft
---

# How-to: Use the HL7 Results Gateway API

## Introduction

This guide provides step-by-step instructions for laboratory staff and developers to authenticate, submit HL7 messages, and validate results using the HL7 Results Gateway API. All examples use the UK South Azure region. Prerequisites: API access, .http file support (e.g., VS Code REST Client), and a valid HL7 message.

## 1. Authenticate to the API

> **Note:** If your API requires authentication (API key, Azure AD, etc.), insert the authentication method here. If not required, skip this section.

### Example: API Key Authentication

Create a file `auth.http`:

```http
### Authenticate (replace with your actual endpoint and key)
GET https://<your-api-url>/api/health
Ocp-Apim-Subscription-Key: {{API_KEY}}
```

- Replace `{{API_KEY}}` with your actual key.
- Save and run this request in your .http client.
- If successful, you should receive a 200 OK response.

## 2. Submit an HL7 Message

Use the `/api/hl7/process` endpoint to submit HL7 messages.

### Example: process-hl7-message.http

```http
### Submit HL7 Message
POST https://<your-api-url>/api/hl7/process
Content-Type: text/plain
Ocp-Apim-Subscription-Key: {{API_KEY}}

MSH|^~\&|... (your HL7 message here)
```

- Replace `<your-api-url>` and `{{API_KEY}}` as appropriate.
- Paste your HL7 message in the body.
- A successful response will include a status and parsed message details.

## 3. Check API Health

Use the `/api/health` endpoint to verify the API is running.

### Example: health-check.http

```http
### Health Check
GET https://<your-api-url>/api/health
```

- A 200 OK response indicates the API is healthy.

## 4. Validate and Interpret Results

- Review the JSON response for `success`, `parsedMessage`, and any error details.
- If `success` is false, check the `error` field for troubleshooting.
- Common errors: invalid HL7 format, missing fields, authentication failure.

## 5. Testing with .http Files

- Use the provided `.http` files (`health-check.http`, `process-hl7-message.http`, and `auth.http` if needed) in VS Code or compatible tools.
- Edit endpoints and keys as needed for your environment.
- Run requests and verify responses directly in your editor.

## 6. Troubleshooting & FAQ

- **401 Unauthorized:** Check your API key or authentication method.
- **400 Bad Request:** Ensure your HL7 message is valid and properly formatted.
- **500 Internal Server Error:** Contact support with the error details.
- For further help, see the [Project README](../README.md) or contact the API maintainer.

## 7. Further Reading & References

- [Diátaxis Documentation Framework](https://diataxis.fr/)
- [HL7 Results Gateway Copilot Instructions](../.github/copilot-instructions.md)
- [Azure Functions API Reference](https://learn.microsoft.com/en-us/azure/azure-functions/)
- [Project README](../README.md)

---

**Note:** Do not include real credentials or sensitive data in requests. All examples use the UK South Azure region.
