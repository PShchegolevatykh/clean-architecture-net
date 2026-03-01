#!/bin/bash
# publish.sh

echo "🚀 Stopping existing containers..."
docker compose down

echo "🏗️  Building image..."
docker compose build

echo "🚢 Deploying new version..."
docker compose up -d

echo "✅ Done! Application is running."