function toggleDetails(loanType) {
    const details = document.getElementById(loanType + 'Details');
    const btn = document.getElementById(loanType + 'Btn');
    const icon = document.getElementById(loanType + 'Icon');

    if (details.classList.contains('show')) {
        details.classList.remove('show');
        btn.textContent = 'Read more';
        icon.textContent = '▼';
    } else {
        details.classList.add('show');
        btn.textContent = 'Read less';
        icon.textContent = '▲';
    }
}

// Loan eligibility calculator
document.getElementById("eligibilityForm").addEventListener("submit", function (e) {
    e.preventDefault();

    const revenue = parseFloat(document.getElementById("annualRevenue").value);
    const loans = parseFloat(document.getElementById("existingLoans").value);
    const request = parseFloat(document.getElementById("requestedLoan").value);
    const maxEligible = revenue * 0.6 - loans;

    const result = document.getElementById("result");
    result.classList.remove("hidden");

    if (request <= maxEligible && maxEligible > 0) {
        result.className = "vista-alert-success";
        result.innerHTML = `
                        <div style="display: flex; align-items: center; gap: 0.75rem;">
                            <span style="font-size: 1.5rem;">✅</span>
                            <p style="font-weight: 500; margin: 0;">Eligible! You can apply for up to ₹${maxEligible.toLocaleString("en-IN")}</p>
                        </div>
                    `;
    } else {
        result.className = "vista-alert-error";
        result.innerHTML = `
                        <div style="display: flex; align-items: center; gap: 0.75rem;">
                            <span style="font-size: 1.5rem;">❌</span>
                            <p style="font-weight: 500; margin: 0;">Not eligible. Maximum eligible amount: ₹${Math.max(0, maxEligible).toLocaleString("en-IN")}</p>
                        </div>
                    `;
    }
});