# Termii API Coverage

This document tracks the Termii API surface against SDK implementation work. It should be updated in the same PR that adds or changes endpoint support.

## Sources

- Termii developer docs: https://developer.termii.com/
- Termii Messaging docs: https://developer.termii.com/messaging
- Termii Token docs: https://developer.termii.com/token
- Termii Insights docs: https://developer.termii.com/insights
- Termii Postman collection: https://termii.s3.us-west-1.amazonaws.com/upload/files/UozvGXj5czYEeY4OmE2f.json

The Termii docs describe a REST/JSON API and state that each account has its own base URL. The current documentation pages list an update date of April 14, 2026 for the main product areas.

## Status Legend

| Status | Meaning |
| --- | --- |
| Implemented | SDK behavior exists, has tests, and is documented. |
| In progress | Work is actively represented by an open issue or PR. |
| Planned | Endpoint is in scope but not implemented yet. |
| Deferred | Endpoint exists in Termii docs, but is outside the first SDK milestone. |
| Needs verification | Endpoint appears in one source but needs confirmation against docs or live behavior before implementation. |

## Foundation Coverage

| Capability | SDK status | Tracking |
| --- | --- | --- |
| Solution, SDK project, unit tests, integration test shell, examples project | Implemented | #1, PR #15 |
| Client configuration, `HttpClient` pipeline, API-key injection, DI registration | Implemented | #2, PR #17 |
| API coverage matrix | Implemented | #14, PR #18 |
| Error handling and request validation | Implemented | #6, PR #19 |
| Reusable fake HTTP test helpers and opt-in live test conventions | Implemented | #10, PR #20 |
| README endpoint examples | In progress | #12 |
| CI build/test/package validation | Implemented | #11, PR #22 |
| NuGet package metadata and publishing workflow | Implemented | #8, PR #23 |
| First GitHub Release and NuGet publish | Implemented | #13, PR #28, v0.2.0 |

## Endpoint Coverage

| Area | Capability | Method | Path | Auth placement | SDK surface | SDK status | Tracking | Test status |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| Messaging | Fetch sender IDs | GET | `/api/sender-id` | Query | `TermiiClient.SenderIds` | Implemented | #3, PR #24 | Unit tests added |
| Messaging | Request sender ID | POST | `/api/sender-id/request` | JSON body | `TermiiClient.SenderIds` | Implemented | #3, PR #24 | Unit tests added |
| Messaging | Send SMS/channel message | POST | `/api/sms/send` | JSON body | `TermiiClient.Messaging` | Implemented | #7, PR #21 | Unit tests added |
| Messaging | Send WhatsApp conversational message | POST | `/api/sms/send` | JSON body | `TermiiClient.Messaging` | Implemented | #7, PR #21 | Unit tests added |
| Messaging | Send bulk message | POST | `/api/sms/send/bulk` | JSON body | `TermiiClient.Messaging` | Implemented | #7, PR #21 | Unit tests added |
| Messaging | Send via Number API | POST | `/api/sms/number/send` | JSON body | `TermiiClient.Numbers` | Implemented | #5, PR #25 | Unit tests added |
| Token | Send OTP token | POST | `/api/sms/otp/send` | JSON body | `TermiiClient.Tokens` | Implemented | #4, PR #26 | Unit tests added |
| Token | Verify OTP token | POST | `/api/sms/otp/verify` | JSON body | `TermiiClient.Tokens` | Implemented | #4, PR #26 | Unit tests added |
| Token | Generate in-app OTP token | POST | `/api/sms/otp/generate` | JSON body | `TermiiClient.Tokens` | Implemented | #4, PR #26 | Unit tests added |
| Token | Send voice OTP | POST | `/api/sms/otp/send/voice` | JSON body | `TermiiClient.Tokens` | Implemented | #4, PR #26 | Unit tests added |
| Token | Send voice call token | POST | `/api/sms/otp/call` | JSON body | `TermiiClient.Tokens` | Implemented | #4, PR #26 | Unit tests added |
| Token | Send email OTP token | POST | `/api/email/otp/send` | JSON body | `TermiiClient.Tokens` | Implemented | #4, PR #26 | Unit tests added |
| Token | Send WhatsApp OTP token | POST | `/api/sms/send` | JSON body with `channel=whatsapp_otp` | `TermiiClient.Tokens` | Implemented | #4, PR #26 | Unit tests added |
| Insights | Get balance | GET | `/api/get-balance` | Query | `TermiiClient.Insights` | Implemented | #9, PR #27 | Unit tests added |
| Insights | Search DND/number status | GET | `/api/check/dnd` | Query | `TermiiClient.Insights` | Implemented | #9, PR #27 | Unit tests added |
| Insights | Query number intelligence/status | GET | `/api/insight/number/query` | Query | `TermiiClient.Insights` | Implemented | #9, PR #27 | Unit tests added |
| Insights | Fetch message inbox/history | GET | `/api/sms/inbox` | Query | `TermiiClient.Insights` | Implemented | #9, PR #27 | Unit tests added |
| Insights | Fetch message analytics/history | GET | `/api/sms/history/analytics` | Query | `TermiiClient.Insights` | In progress | #34 | Unit tests added |

## Deferred Coverage

The following documented APIs are useful but should come after the first SDK milestone unless a consumer need pushes them forward.

| Area | Capability | Method | Path | Status | Notes |
| --- | --- | --- | --- | --- | --- |
| Messaging | Send WhatsApp template without media | POST | `/api/send/template` | Deferred | Requires WhatsApp template/device setup. |
| Messaging | Send WhatsApp template with media | POST | `/api/send/template/media` | Deferred | Requires WhatsApp template/device setup and media payload modeling. |
| Campaigns | Fetch phonebooks | GET | `/api/phonebooks` | Deferred | Part of campaign/phonebook management, not core messaging. |
| Campaigns | Create phonebook | POST | `/api/phonebooks` | Deferred | Part of campaign/phonebook management. |
| Campaigns | Update phonebook | PATCH | `/api/phonebooks/{phonebook_id}` | Deferred | Part of campaign/phonebook management. |
| Campaigns | Delete phonebook | DELETE | `/api/phonebooks/{phonebook_id}` | Deferred | Part of campaign/phonebook management. |
| Email | Send product notification email | POST | `/api/templates/send-email` | Deferred | Email notifications should be a separate milestone after SMS/token/insights. |
| Insights | Webhook events and reports | N/A | Consumer webhook endpoint | Implemented | Receiver-side model support and README example covered by #32. |

## Postman Collection Reconciliation

The provided Postman collection currently lists these groups:

| Collection group | Requests observed | Coverage decision |
| --- | --- | --- |
| Switch | SenderId, Numbers, Send | Covered by #3, #5, and #7. |
| Token | Send Token, Verify Token, In-App Token | Covered by #4. |
| Insight | history analytics, Search, Balance, Inbox API | Covered by #9 and #34. |

## Implementation Rules

- Every implemented endpoint should have request-building unit tests using fake HTTP handlers.
- Live integration tests must be opt-in and gated by environment variables.
- Public models should be typed, nullable-aware, and tolerant of undocumented optional response fields.
- Endpoints that require account activation, WhatsApp devices, sender approval, or spendable credits should not run in CI by default.
- README examples should be added or updated with each endpoint group.
