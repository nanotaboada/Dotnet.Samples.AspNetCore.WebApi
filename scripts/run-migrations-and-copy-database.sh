#!/bin/bash

DATA_FILE="players-sqlite3.db"

PROJECT_ROOT_PATH="src/Dotnet.Samples.AspNetCore.WebApi"
PROJECT_BASE_PATH="$PROJECT_ROOT_PATH/bin/Debug/net8.0"

SOURCE_FILE_PATH="$PROJECT_BASE_PATH/storage/$DATA_FILE"
TARGET_FILE_PATH="$PROJECT_ROOT_PATH/storage/$DATA_FILE"

log() {
    local emoji=$1
    local level=$2
    local message=$3
    local timestamp
    timestamp=$(date +"%Y-%m-%d %H:%M:%S")
    echo "$emoji [$timestamp] [$level] $message"
}

# Check if the EF Core CLI tool is installed
if ! command -v dotnet ef &> /dev/null; then
    log "❌" "ERROR" "'dotnet ef' not found. Install it with 'dotnet tool install --global dotnet-ef'"
    exit 1
fi

# Ensure clean placeholder database file exists
log "✅" "INFO" "Resetting placeholder database at '$TARGET_FILE_PATH'"
rm -f "$TARGET_FILE_PATH"
touch "$TARGET_FILE_PATH"

# Run the database migration
log "✅" "INFO" "Running EF Core database migration for project at '$PROJECT_ROOT_PATH'..."
dotnet ef database update --project "$PROJECT_ROOT_PATH"
if [ $? -ne 0 ]; then
    log "❌" "ERROR" "Migration failed. See error above."
    exit 1
fi

# Check and copy database
if [ -f "$SOURCE_FILE_PATH" ]; then
    log "✅" "INFO" "Found database at '$SOURCE_FILE_PATH'"
    log "✅" "INFO" "Copying to '$TARGET_FILE_PATH'..."
    cp -f "$SOURCE_FILE_PATH" "$TARGET_FILE_PATH"

    if [ $? -eq 0 ]; then
        log "✅" "INFO" "Database successfully copied to '$TARGET_FILE_PATH'"
    else
        log "❌" "ERROR" "Failed to copy the database file."
        exit 1
    fi
else
    log "⚠️" "WARNING" "Database file not found at '$SOURCE_FILE_PATH'."
    log "⚠️" "WARNING" "Make sure the migration actually generated the file."
    exit 1
fi

# Confirm destination file exists
if [ -f "$TARGET_FILE_PATH" ]; then
    log "✅" "INFO" "Done. The database is now available at '$TARGET_FILE_PATH'"
else
    log "⚠️" "WARNING" "Something went wrong. The destination file was not found."
    exit 1
fi
