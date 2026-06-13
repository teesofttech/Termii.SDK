# Releasing

This project publishes the SDK as a NuGet package from version tags.

## Versioning

- Use semantic versioning.
- Keep `VersionPrefix` in `src/Termii/Termii.csproj` aligned with the release tag.
- Use preview suffixes for pre-release packages, for example `0.1.0-preview.1`.

## Release Checklist

1. Confirm all intended issues for the release are merged.
2. Confirm CI is green on `main`.
3. Update package metadata and README examples if needed.
4. Update `VersionPrefix` in `src/Termii/Termii.csproj`.
5. Create and push a tag like `v0.1.0`.
6. Confirm the release workflow creates the package artifact.
7. Confirm NuGet publishing succeeds when the `NUGET_API_KEY` secret is configured.

## Required Secrets

| Secret | Purpose |
| --- | --- |
| `NUGET_API_KEY` | API key used by the release workflow to publish to NuGet.org. |

The release workflow skips the publish step when `NUGET_API_KEY` is not set, but still builds, tests, packs, and uploads the package artifact.
