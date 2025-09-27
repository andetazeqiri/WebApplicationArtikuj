#!/bin/bash



# Colors for output
GREEN='\033[0;32m'
BLUE='\033[0;34m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Get the directory where the script is located
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

echo -e "${BLUE}📁 Project directory: ${SCRIPT_DIR}${NC}"

# Function to cleanup background processes
cleanup() {
    echo -e "\n${YELLOW}🛑 Stopping applications...${NC}"
    kill $(jobs -p) 2>/dev/null
    exit 0
}

# Trap Ctrl+C to cleanup
trap cleanup INT

# Start Backend API
echo -e "${GREEN}🔧 Starting Backend API on http://localhost:5276...${NC}"
cd "${SCRIPT_DIR}/Backend/ArtikujManager.API"
dotnet run --urls="http://localhost:5276" &
BACKEND_PID=$!

# Wait a moment for backend to start
sleep 3

# Start Frontend MVC
echo -e "${GREEN}🌐 Starting Frontend MVC on http://localhost:5001...${NC}"
cd "${SCRIPT_DIR}/Frontend/ArtikujManager.Web"
dotnet run --urls="http://localhost:5001" &
FRONTEND_PID=$!

echo ""
echo -e "${GREEN}✅ Both applications are starting...${NC}"
echo -e "${BLUE}📊 Backend API (Swagger): http://localhost:5276/swagger${NC}"
echo -e "${BLUE}🌐 Frontend Web App: http://localhost:5001${NC}"
echo ""
echo -e "${YELLOW}ℹ️  Default Admin Login:${NC}"
echo -e "   Email: admin@artikujmanager.com"
echo -e "   Password: Admin123!"
echo ""
echo -e "${YELLOW}Press Ctrl+C to stop both applications${NC}"

# Wait for both processes
wait $BACKEND_PID $FRONTEND_PID