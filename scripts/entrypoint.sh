#!/bin/sh
set -e

# Helper function for formatted logging
log() {
    local message="$1"
    echo "[ENTRYPOINT] $(date '+%Y/%m/%d - %H:%M:%S') | $message"
    return 0
}

IMAGE_STORAGE_PATH="/app/hold/players-sqlite3.db"
VOLUME_STORAGE_PATH="/storage/players-sqlite3.db"

log "✔ Starting container..."

if [ ! -f "$VOLUME_STORAGE_PATH" ]; then
    log "⚠️ No existing database file found in volume."
    if [ -f "$IMAGE_STORAGE_PATH" ]; then
        log "🔄 Copying database file to writable volume..."
        cp "$IMAGE_STORAGE_PATH" "$VOLUME_STORAGE_PATH"
        log "✔ Database initialized at $VOLUME_STORAGE_PATH"
    else
        log "⚠️ Database file missing at $IMAGE_STORAGE_PATH"
        exit 1
    fi
else
    log "✔ Existing database file found. Skipping seed copy."
fi

log "✔ Ready!"
log "🚀 Launching app..."
# Derive the API URL from ASPNETCORE_URLS (first entry if semicolon-separated),
# replacing the wildcard host (+, 0.0.0.0) with localhost for display purposes.
_raw_url=$(printf '%s' "${ASPNETCORE_URLS:-http://+:9000}" | cut -d';' -f1)
API_URL=$(printf '%s' "$_raw_url" | sed 's|+|localhost|g; s|0\.0\.0\.0|localhost|g')
log "🔌 API endpoints | $API_URL"
exec "$@"
