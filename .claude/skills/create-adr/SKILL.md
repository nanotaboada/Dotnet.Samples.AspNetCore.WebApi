---
name: create-adr
description: Write a new Architecture Decision Record in docs/adr/ using the project template and numbering, or supersede an existing one. Use when a change introduces or reverses an architectural, technology, API contract, or workflow decision.
---

# Create an ADR

1. Check `docs/adr/README.md`: confirm the decision qualifies ("When to Create
   an ADR") and find the next number in the Index.
2. Create `docs/adr/NNNN-kebab-case-title.md` from the Template section of
   `docs/adr/README.md`: Date, Status, Context, Decision ("We will..."),
   Consequences (Positive / Negative / Neutral).
3. Never edit an accepted ADR's decision. To replace one, set the old ADR's
   Status to `Superseded by ADR-NNNN` and link back from the new one.
4. Add the new row to the Index table in `docs/adr/README.md` and update the
   ADR range mentioned in `CLAUDE.md` ("ADR-0001–NNNN").
5. Reference the ADR from the issue and the CHANGELOG entry.
