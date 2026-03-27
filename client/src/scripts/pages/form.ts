// SWITCH FORMS
const buttons = document.querySelectorAll(".form-switch__btn");
const forms = document.querySelectorAll(".form");

buttons.forEach(btn => {
    btn.addEventListener("click", () => {

        buttons.forEach(b => b.classList.remove("active"));
        btn.classList.add("active");

        const target = btn.getAttribute("data-target");

        forms.forEach(form => {
            form.classList.remove("active");
        });

        document.getElementById(`${target}-form`)?.classList.add("active");
    });
});

// TEMPLATE SELECT
document.querySelectorAll(".template-grid").forEach(grid => {
const cards = grid.querySelectorAll(".template-card");

cards.forEach(card => {
        card.addEventListener("click", () => {
        cards.forEach(c => c.classList.remove("active"));
        card.classList.add("active");
        });
    });
});