---
name: create-issue
description: Draft a GitHub Issue (feature request or bug report) as the Spec-Driven Development spec artifact before implementing a change in this project. Use when the user wants to start a feature, report a bug, or asks to open a ticket.
---

# Create an issue

1. Discuss the change in Plan mode until the problem and the approach are clear.
2. Pick the template and fill every section:
   - Feature (`enhancement` label): `.github/ISSUE_TEMPLATE/feature_request.md`
     — Problem · Proposed Solution · Suggested Approach _(optional)_ ·
     Acceptance Criteria · References
   - Bug (`bug` label): `.github/ISSUE_TEMPLATE/bug_report.md` —
     Description · Steps to Reproduce · Expected Behavior · Actual Behavior ·
     Environment · Additional Context · Possible Solution _(optional)_
3. Write acceptance criteria as verifiable checkboxes; reference real files and
   ADRs (`docs/adr/`).
4. Show the draft to the user. Create it only after explicit approval:

   ```bash
   gh issue create --title "<title>" --label <enhancement|bug> --label .NET --body-file <draft.md>
   ```

5. Use the issue number in the branch name (`type/<number>-short-description`)
   and in the commit subject (`(#<number>)`).
