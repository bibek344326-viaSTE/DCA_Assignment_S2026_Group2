# Custom View: Event Guest Status Overview

This view supports an event creator who needs to manage guest involvement for one event.

```text
+---------------------------------------------------------------+
| Event title                                      Attendees n/m |
+----------------------+----------------------+-----------------+
| Participants         | Invitations          | Join requests   |
| - Guest name         | Pending              | Pending         |
| - Guest name         | - Guest name         | - Guest + reason|
|                      | Declined             | Accepted        |
|                      | - Guest name         | - Guest + reason|
|                      |                      | Declined        |
|                      |                      | - Guest + reason|
+----------------------+----------------------+-----------------+
```

Data needed:

- Event id and title.
- Participants.
- Pending and declined invitations.
- Pending, accepted, and declined join requests.
- Guest name and email for each listed guest.
- Join request reason where relevant.

Implemented by `GetEventGuestStatusOverviewQuery`.
