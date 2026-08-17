**English** | [Русский](SECURITY.ru.md)

# Security Policy

## ⚠️ Important Notice (Disclaimer)

> **WARNING**: This Wialon API SDK library was written and generated using **Artificial Intelligence (AI)**.  
> The project is currently in the **active testing, refinement, and validation stage**.  
> **Use of this library is strictly at your own risk.**

---

### Core Provisions

1. **Testing and Validation**:
   - Despite having an automated test suite (xUnit), the library may contain subtle bugs, data model inaccuracies, unexpected network layer behavior, or unhandled exceptions.
   - It is **strongly recommended** to conduct extensive testing in an isolated test environment (Sandbox) before deploying to critical or production systems.

2. **Handling Tokens and Secrets**:
   - The Wialon Access Token (`WIALON_ACCESS_TOKEN`) grants access to your units, users, and resources according to the assigned permissions.
   - **Never commit your `.env` file to public repositories.** Verify that `.env` is listed in `.gitignore`.
   - Use tokens with the minimal required permissions (`access_type` / `fl`) and limited lifetimes.

3. **Limitation of Liability**:
   - The authors and maintainers of this project are not liable for any direct or indirect damages, data loss, incorrect telematics command execution, account suspension, or any other consequences resulting from the use of this SDK.

---

## 🛡 Reporting Vulnerabilities and Bugs

If you discover a security vulnerability or critical issue in this SDK:

1. **Do not open a public issue** containing confidential information or exploits.
2. Create a private Security Advisory on GitHub or contact the repository maintainer directly.
3. Include reproduction steps and describe the expected impact.
