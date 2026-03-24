Run the pre-commit checklist for this project:

1. Remind me to update `CHANGELOG.md` `[Unreleased]` section (Added / Changed / Fixed / Removed) — I must do this manually.
2. Run `dotnet build --configuration Release` — must succeed.
3. Run `dotnet test --settings .runsettings` — all tests must pass.
4. Run `dotnet csharpier --check .` — must pass (run `dotnet csharpier .` to auto-fix).

Run steps 2–4, report the results clearly, then propose a branch name and commit message following Conventional Commits format for my approval. Do not create the branch or commit until I explicitly confirm.
