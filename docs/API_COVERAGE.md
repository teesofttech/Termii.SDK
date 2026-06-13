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
| API coverage matrix | In progress | #14 |
| Error handling and request validation | Planned | #6 |
| Reusable fake HTTP test helpers and opt-in live test conventions | Planned | #10 |
| README endpoint examples | Planned | #12 |
| CI build/test/package validation | Planned | #11 |
| NuGet package metadata and publishing workflow | Planned | #8 |
| First GitHub Release and NuGet publish | Planned | #13 |

## Endpoint Coverage

| Area | Capability | Method | Path | Auth placement | SDK surface | SDK status | Tracking | Test status |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| Messaging | Fetch sender IDs | GET | `/api/sender-id` | Query | `TermiiClient.SenderIds` | Planned | #3 | Planned unit + optional integration |
| Messaging | Request sender ID | POST | `/api/sender-id/request` | JSON body | `TermiiClient.SenderIds` | Planned | #3 | Planned unit + optional integration |
| Messaging | Send SMS/channel message | POST | `/api/sms/send` | JSON body | `TermiiClient.Messaging` | Planned | #7 | Planned unit + optional integration |
| Messaging | Send WhatsApp conversational message | POST | `/api/sms/send` | JSON body | `TermiiClient.Messaging` | Planned | #7 | Planned unit + optional integration |
| Messaging | Send bulk message | POST | `/api/sms/send/bulk` | JSON body | `TermiiClient.Messaging` | Planned | #7 | Planned unit + optional integration |
| Messaging | Send via Number API | POST | `/api/sms/number/send` | Query or JSON body, docs/examples vary | `TermiiClient.Messaging` or `TermiiClient.Numbers` | Planned | #5 | Planned unit + optional integration |
| Token | Send OTP token | POST | `/api/sms/otp/send` | JSON body | `TermiiClient.Token` | Planned | #4 | Planned unit + optional integration |
| Token | Verify OTP token | POST | `/api/sms/otp/verify` | JSON body | `TermiiClient.Token` | Planned | #4 | Planned unit + optional integration |
| Token | Generate in-app OTP token | POST | `/api/sms/otp/generate` | JSON body | `TermiiClient.Token` | Planned | #4 | Planned unit + optional integration |
| Token | Send voice OTP/call token | POST | `/api/sms/otp/call` | JSON body | `TermiiClient.Token` | Planned | #4 | Planned unit + optional integration |
| Token | Send email OTP token | POST | `/api/email/otp/send` | JSON body | `TermiiClient.Token` | Planned | #4 | Planned unit + optional integration |
| Token | Send WhatsApp OTP token | POST | `/api/sms/send` | JSON body with `channel=whatsapp_otp` | `TermiiClient.Token` | Planned | #4 | Planned unit + optional integration |
| Insights | Get balance | GET | `/api/get-balance` | Query | `TermiiClient.Insights` | Planned | #9 | Planned unit + optional integration |
| Insights | Search DND/number status | GET | `/api/check/dnd` | Query | `TermiiClient.Insights` | Planned | #9 | Planned unit + optional integration |
| Insights | Query number intelligence/status | GET | `/api/insight/number/query` | Query | `TermiiClient.Insights` | Planned | #9 | Planned unit + optional integration |
| Insights | Fetch message inbox/history | GET | `/api/sms/inbox` | Query | `TermiiClient.Insights` | Planned | #9 | Planned unit + optional integration |
| Insights | Fetch message analytics/history | GET | `/api/sms/history/analytics` | Query | `TermiiClient.Insights` | Needs verification | #9 | Planned unit + optional integration |

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
| Insights | Webhook events and reports | N/A | Consumer webhook endpoint | Deferred | This is SDK model/documentation support rather than an outbound Termii API call. |

## Postman Collection Reconciliation

The provided Postman collection currently lists these groups:

| Collection group | Requests observed | Coverage decision |
| --- | --- | --- |
| Switch | SenderId, Numbers, Send | Covered by #3, #5, and #7. |
| Token | Send Token, Verify Token, In-App Token | Covered by #4. |
| Insight | history analytics, Search, Balance, Inbox API | Covered by #9, with `/api/sms/history/analytics` marked for verification against live behavior/docs. |

## Implementation Rules

- Every implemented endpoint should have request-building unit tests using fake HTTP handlers.
- Live integration tests must be opt-in and gated by environment variables.
- Public models should be typed, nullable-aware, and tolerant of undocumented optional response fields.
- Endpoints that require account activation, WhatsApp devices, sender approval, or spendable credits should not run in CI by default.
- README examples should be added or updated with each endpoint group.
