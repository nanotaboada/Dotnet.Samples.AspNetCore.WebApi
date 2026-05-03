#!/bin/sh
set -e

# Helper function for formatted logging
log() {
    local message="$1"
    echo "[ENTRYPOINT] $(date '+%Y/%m/%d - %H:%M:%S') | $message"
    return 0
}

log "✔ Starting container..."

DATABASE_PROVIDER="${DATABASE_PROVIDER:-sqlite}"

if [ "$DATABASE_PROVIDER" = "postgres" ]; then
    log "✔ Using PostgreSQL database."
else
    VOLUME_STORAGE_PATH="${STORAGE_PATH:-/storage/players-sqlite3.db}"

    if [ ! -f "$VOLUME_STORAGE_PATH" ]; then
        log "⚠️ No existing database file found in volume."
        log "🗄️ EF Core migrations will initialize the database on first start."
    else
        log "✔ Existing database file found at $VOLUME_STORAGE_PATH."
    fi
fi

log "✔ Ready!"
log "🚀 Launching app..."
# Derive the API URL from ASPNETCORE_URLS (first entry if semicolon-separated),
# replacing the wildcard host (+, 0.0.0.0) with localhost for display purposes.
_raw_url=$(printf '%s' "${ASPNETCORE_URLS:-http://+:9000}" | cut -d';' -f1)
API_URL=$(printf '%s' "$_raw_url" | sed 's|+|localhost|g; s|0\.0\.0\.0|localhost|g')
log "🔌 API endpoints | $API_URL"
exec "$@"
