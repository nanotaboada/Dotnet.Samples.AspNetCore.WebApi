Run the pre-release checklist for this project. Work through all three phases
in order, pausing for explicit confirmation at each decision point before
proceeding. Never create a branch, commit, tag, or push without approval.

---

## Phase 1 — Determine next release

1. Run `git status` and confirm the working tree is clean and on `master`.
   If not, stop and report the problem.

2. Run `git tag --sort=-v:refname` to list existing tags. Identify the most
   recent tag matching `v*.*.*-*` and extract its stadium codename.

3. Read the A–Z stadium table from `CHANGELOG.md` to find the next stadium:
   - **No tags yet**: start at `A` (first stadium in the table).
   - **Normal case**: use the stadium that follows the last used codename
     alphabetically. If letters were skipped, pick the next after the
     highest existing codename — do not backfill gaps.
   - **Last codename is `Z`** (Zentralstadion): the list is finite. Stop and
     refer to ADR 0012 for guidance on extending or revisiting the convention.

4. Read the `[Unreleased]` section of `CHANGELOG.md` and infer the version
   bump using these rules (applied in order — first match wins):
   - Any entry contains the word **BREAKING** (case-insensitive), a
     `BREAKING CHANGE:` token in a commit footer, or a `!` suffix after
     the commit type/scope (e.g. `feat!:` or `feat(scope)!:`) → **major** bump
   - Any `### Added` subsection has entries → **minor** bump
   - Otherwise (only `### Changed`, `### Fixed`, `### Removed`) → **patch** bump

5. Compute the next version by applying the bump to the current latest tag's
   semver (e.g. `v2.1.0-dusseldorf` + minor → `2.2.0`).

6. Present a summary for confirmation before continuing:
   - Last tag and stadium
   - Next version and stadium codename
   - Bump type and the reasoning (what triggered it)
   - Proposed tag: `vX.Y.Z-{stadium}`
   - Proposed branch: `release/vX.Y.Z-{stadium}`

   **Wait for explicit approval before proceeding to Phase 2.**

---

## Phase 2 — Prepare release branch

1. Create branch `release/vX.Y.Z-{stadium}` from `master`.

2. Edit `CHANGELOG.md`:
   - Replace `## [Unreleased]` with `## [X.Y.Z - StadiumName] - YYYY-MM-DD`
     (use today's date; use the stadium's display name from the table, e.g.
     "Bernabeu", "Centenario").
   - Consolidate duplicate subsection headings (e.g. two `### Added` blocks
     should be merged into one).
   - Add a new empty `## [Unreleased]` section at the top (above the new
     versioned heading) with the standard subsections.
   - Update the compare links at the bottom of the file:
     - `[unreleased]` → `.../compare/vX.Y.Z-{stadium}...HEAD`
     - Add `[X.Y.Z - StadiumName]` → `.../compare/v{prev-tag}...vX.Y.Z-{stadium}`

3. Show the full diff of `CHANGELOG.md`.

4. If `coderabbit` CLI is installed, run `coderabbit review --type uncommitted --prompt-only`
   on the uncommitted CHANGELOG changes:
   - If actionable/serious findings are reported, stop and address them before proceeding.
   - If only nitpick-level findings, report them and continue.
   - If `coderabbit` is not installed, skip with a note.

5. Propose this commit message:

   ```text
   docs(changelog): prepare release notes for vX.Y.Z-{stadium} (#issue)
   ```

   **Wait for explicit approval before committing.**

6. Run `dotnet build --configuration Release` — must succeed.

7. Run `dotnet test --settings .runsettings` — all tests must pass.

8. If `dotnet csharpier` is available, run `dotnet csharpier --check .` — must pass
   (run `dotnet csharpier .` to auto-fix). Skip with a note if not installed.

9. Propose opening a PR from `release/vX.Y.Z-{stadium}` into `master`.
   **Wait for explicit approval before opening.**

10. Open the PR with:
   - Title: `docs(changelog): prepare release notes for vX.Y.Z-{stadium}`
   - Body summarising what is included in this release.

---

## Phase 3 — Tag and release

1. Wait — do not proceed until the user confirms:
   - CI is green
   - The PR has been merged into `master`

2. Once confirmed, run:
   ```bash
   git checkout master && git pull origin master
   ```
   and show the resulting `git log --oneline -3`.

3. Propose the annotated tag:
   ```bash
   git tag -a vX.Y.Z-{stadium} -m "Release X.Y.Z - StadiumName"
   ```

   **Wait for explicit approval before creating the tag.**

4. Create the tag, then propose:
   ```bash
   git push origin vX.Y.Z-{stadium}
   ```

   **Wait for explicit approval before pushing.** Remind the user that pushing
   the tag triggers the CD workflow which will build, publish the Docker image,
   and create the GitHub Release.
