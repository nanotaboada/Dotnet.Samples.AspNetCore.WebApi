#!/bin/bash
# 📊 Token Counter for Copilot Instruction Files
# Uses tiktoken (OpenAI's tokenizer) for accurate counting
# Approximation: ~0.75 words per token (English text)

set -e

echo "📊 Token Analysis for Copilot Instructions"
echo "=========================================="
echo ""

# Check if tiktoken is available
if command -v python3 &> /dev/null; then
    # Try to use tiktoken for accurate counting
    if python3 -c "import tiktoken" 2>/dev/null; then
        echo "✅ Using tiktoken (accurate Claude/GPT tokenization)"
        echo ""
    else
        # tiktoken not found - offer to install
        echo "⚠️  tiktoken not installed"
        echo ""

        # Detect non-interactive environment (CI/CD)
        if [ ! -t 0 ] || [ -n "$CI" ] || [ -n "$CI_CD" ]; then
            echo "🤖 Non-interactive environment detected (CI/CD)"
            echo "📝 Using word-based approximation"
            echo "   (To auto-install in CI, set AUTO_INSTALL_TIKTOKEN=1)"
            echo ""
            USE_APPROX=1
        elif [ -n "$AUTO_INSTALL_TIKTOKEN" ]; then
            echo "📥 Installing tiktoken (AUTO_INSTALL_TIKTOKEN=1)..."
            if pip3 install tiktoken --quiet; then
                echo "✅ tiktoken installed successfully!"
                echo ""
                # Re-run the script after installation
                exec "$0" "$@"
            else
                echo "❌ Installation failed. Using word-based approximation instead."
                echo ""
                USE_APPROX=1
            fi
        else
            echo "tiktoken provides accurate token counting for Claude/GPT models."
            read -p "📦 Install tiktoken now? (y/n): " -n 1 -r
            echo ""
            if [[ $REPLY =~ ^[Yy]$ ]]; then
                echo "📥 Installing tiktoken..."
                if pip3 install tiktoken --quiet; then
                    echo "✅ tiktoken installed successfully!"
                    echo ""
                    # Re-run the script after installation
                    exec "$0" "$@"
                else
                    echo "❌ Installation failed. Using word-based approximation instead."
                    echo ""
                    USE_APPROX=1
                fi
            else
                echo "📝 Using word-based approximation instead"
                echo "   (Install manually: pip3 install tiktoken)"
                echo ""
                USE_APPROX=1
            fi
        fi
    fi

    # Only run tiktoken if it's available and we didn't set USE_APPROX
    if [ -z "$USE_APPROX" ] && python3 -c "import tiktoken" 2>/dev/null; then

        # Create temporary Python script
        cat > /tmp/count_tokens.py << 'PYTHON'
import tiktoken
import sys

# cl100k_base is used by GPT-4, Claude uses similar tokenization
encoding = tiktoken.get_encoding("cl100k_base")

file_path = sys.argv[1]
with open(file_path, 'r', encoding='utf-8') as f:
    content = f.read()

tokens = encoding.encode(content)
print(len(tokens))
PYTHON

        # Count tokens for each file
        echo "📄 .github/copilot-instructions.md"
        if [ -f ".github/copilot-instructions.md" ]; then
            COPILOT_TOKENS=$(python3 /tmp/count_tokens.py .github/copilot-instructions.md 2>&1 | grep -v "ERROR:root:code for hash" | tail -1)
            echo "   Tokens: $COPILOT_TOKENS"
        else
            echo "   ⚠️  File not found, skipping"
            COPILOT_TOKENS=0
        fi
        echo ""

        echo "📄 AGENTS.md"
        if [ -f "AGENTS.md" ]; then
            AGENTS_TOKENS=$(python3 /tmp/count_tokens.py AGENTS.md 2>&1 | grep -v "ERROR:root:code for hash" | tail -1)
            echo "   Tokens: $AGENTS_TOKENS"
        else
            echo "   ⚠️  File not found, skipping"
            AGENTS_TOKENS=0
        fi
        echo ""

        # Calculate total
        TOTAL=$((COPILOT_TOKENS + AGENTS_TOKENS))
        echo "📊 Summary"
        echo "   Base load (auto): $COPILOT_TOKENS tokens"
        echo "   On-demand load: $AGENTS_TOKENS tokens"
        echo "   Total (if both): $TOTAL tokens"
        echo ""

        # Check against target
        TARGET=600
        LIMIT=650
        if [ $COPILOT_TOKENS -le $TARGET ]; then
            echo "✅ copilot-instructions.md within target ($TARGET tokens)"
        elif [ $COPILOT_TOKENS -le $LIMIT ]; then
            echo "⚠️  copilot-instructions.md over target but within limit ($LIMIT tokens)"
        else
            echo "❌ copilot-instructions.md exceeds limit! Optimization required."
        fi

        # Calculate savings (guard against division by zero)
        if [ $TOTAL -gt 0 ]; then
            SAVINGS=$((AGENTS_TOKENS * 100 / TOTAL))
            echo "💡 Savings: ${SAVINGS}% saved when AGENTS.md not needed"
        else
            echo "💡 Savings: 0% (no tokens to count)"
        fi

        # Cleanup
        rm /tmp/count_tokens.py
    fi
else
    echo "❌ Python3 not found"
    echo "   Python 3 is required for token counting"
    echo "   Install from: https://www.python.org/downloads/"
    echo ""
    exit 1
fi

# Fallback: word-based approximation
if [ -n "$USE_APPROX" ]; then
    echo "📄 .github/copilot-instructions.md"
    WORDS=$(wc -w < .github/copilot-instructions.md | tr -d ' ')
    APPROX_TOKENS=$((WORDS * 4 / 3))
    echo "   Words: $WORDS"
    echo "   Approx tokens: $APPROX_TOKENS"
    echo ""

    echo "📄 AGENTS.md"
    WORDS=$(wc -w < AGENTS.md | tr -d ' ')
    APPROX_TOKENS=$((WORDS * 4 / 3))
    echo "   Words: $WORDS"
    echo "   Approx tokens: $APPROX_TOKENS"
    echo ""

    echo "💡 Note: Run script again to install tiktoken for accurate counts"
fi

echo ""
echo "=========================================="
