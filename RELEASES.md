# Releases

Releases follow the pattern `v{SEMVER}-{STADIUM}` (e.g., `v1.0.0-azteca`). Codenames are drawn alphabetically from the [stadium list](CHANGELOG.md#stadium-release-names) of famous football stadiums that hosted FIFA World Cup matches.

## Workflow

### 1. Create a Release Branch

Branch protection prevents direct pushes to `master`, so all release prep goes through a PR:

```bash
git checkout master && git pull
git checkout -b release/v1.0.0-azteca
```

### 2. Update CHANGELOG.md

Move items from `[Unreleased]` to a new release section in [CHANGELOG.md](CHANGELOG.md):

```bash
# Move items from [Unreleased] to new release section
# Example: [1.0.0 - azteca] - 2026-01-22
git add CHANGELOG.md
git commit -m "docs: prepare changelog for v1.0.0-azteca release"
git push origin release/v1.0.0-azteca
# Open a PR, get it reviewed, and merge into master
```

### 3. Create and Push Tag

After the PR is merged, create and push the version tag from `master`:

```bash
git checkout master && git pull
git tag -a v1.0.0-azteca -m "Release 1.0.0 - Azteca"
git push origin v1.0.0-azteca
```

### 4. Automated CD Workflow

Pushing the tag triggers the CD workflow which automatically:

1. Validates the stadium name
2. Builds and tests the project in Release configuration
3. Publishes Docker images to GitHub Container Registry with three tags
4. Creates a GitHub Release with auto-generated changelog from commits

> 💡 Always update CHANGELOG.md before creating the tag. See [CHANGELOG.md](CHANGELOG.md) for the complete stadium list (A-Z) and release history.

## Docker Pull

Each release publishes multiple tags for flexibility:

```bash
# By semantic version (recommended for production)
docker pull ghcr.io/nanotaboada/dotnet-samples-aspnetcore-webapi:1.0.0

# By stadium name (memorable alternative)
docker pull ghcr.io/nanotaboada/dotnet-samples-aspnetcore-webapi:azteca

# Latest release
docker pull ghcr.io/nanotaboada/dotnet-samples-aspnetcore-webapi:latest
```
