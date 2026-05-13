# Assignment 4 - Domain Model Status

## Must-Have Use Cases

| ID | Use Case | Status | Notes |
| --- | --- | --- |
| UC1 | Creator creates a new empty event | Done | Event gets ID, draft status, working title, empty description, private visibility, and max guests `5`. |
| UC2 | Creator updates event title | Done | Validates length and blocks active/cancelled events. Ready events return to draft. |
| UC3 | Creator updates event description | Done | Validates max length and blocks active/cancelled events. Ready events return to draft. |
| UC4 | Creator updates event start/end date and time | Done | Validates order, duration, usable room interval, max 10 hours, future start, and active/cancelled restrictions. |
| UC5 | Creator makes event public | Done | Draft, ready, and active events can be made public. Cancelled events are rejected. |
| UC6 | Creator makes event private | Done | Draft/ready events can be made private. Ready public events return to draft. Active/cancelled events are rejected. |
| UC7 | Creator sets max guests | Done | Supports valid range, active-event increase-only rule, cancelled rejection, and location capacity guard. |
| UC8 | Creator readies event | Done | Requires valid title, description, time, visibility, and max guests; rejects cancelled or past events. |
| UC9 | Creator activates event | Done | Draft events are readied first, ready events become active, active events stay active, cancelled events are rejected. |
| UC10 | Anonymous registers guest account | Done | Validates VIA email, name rules, profile URL, duplicate email through the command handler, and formatting. |
| UC11 | Guest participates in public event | Done | Requires active, public, future event with room and no duplicate participation. |
| UC12 | Guest cancels participation | Done | Removes participation, no-ops if not participating, and rejects started events. |
| UC13 | Creator invites guest to event | Done | Supports ready/active events and rejects draft/cancelled, full, duplicate invite, or already participating guest. |
| UC14 | Guest accepts invitation | Done | Requires active future event, pending invitation, and available capacity. Handles missing invitation, full, cancelled, ready, and past events. |
| UC15 | Guest declines invitation | Done | Supports pending or accepted invitations, records declined state, and rejects missing invitation or cancelled event. |

## Supporting Structure

| Area | Status | Notes |
| --- | --- | --- |
| Base classes | Done | `Entity<TId>`, `AggregateRoot<TId>`, and `ValueObject` are implemented in the Domain project. |
| Strongly typed IDs | Done | `EventId` and `LocationId` are implemented. Guests are identified by `Email`. |
| Domain tests | Done | Assignment 4 feature tests pass for UC1-UC15 plus basic location coverage. |

## Verification

```powershell
dotnet test Tests\UnitTests\UnitTests.csproj --no-restore --filter "FullyQualifiedName~Features.Event|FullyQualifiedName~Features.Guest|FullyQualifiedName~Features.Location"
```

Result: `213` passed, `0` failed.

## Remaining Notes

The full test suite now passes. Dispatcher coverage is documented under Assignment 6.
