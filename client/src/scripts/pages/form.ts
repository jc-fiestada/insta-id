import { ResponseHandler } from "../response-handler/response";
import { showToast } from "../ui/toast";
import { IdResponse } from "../interface/id.response";

const ResHandler : ResponseHandler = new ResponseHandler(); 

// Switch Forms
const buttons = document.querySelectorAll(".form-switch__btn");
const forms = document.querySelectorAll(".form");

// Specific Forms
const studentForm = document.getElementById("student-form") as HTMLFormElement;
const employeeForm = document.getElementById("employee-form") as HTMLFormElement;

// Form Btns
const formSubmitBtn = document.querySelectorAll(".form-btn") as NodeListOf<HTMLButtonElement>;

studentForm.addEventListener("submit", async (e) => {
    e.preventDefault();

    formSubmitBtn.forEach(btn => btn.disabled = true);

    const studentName = document.getElementById("student-name") as HTMLInputElement;
    const studentCourse = document.getElementById("student-course") as HTMLInputElement;
    const studentEmail = document.getElementById("student-email") as HTMLInputElement;
    const studentSchool = document.getElementById("student-school") as HTMLInputElement;
    const studentImage = document.getElementById("student-image") as HTMLInputElement;

    if (studentImage.files === null || studentImage.files.length === 0){
        showToast({
            type: "warning",
            message : "Image must not be empty",
            duration : 3000
        });
        formSubmitBtn.forEach(btn => btn.disabled = false);
        return;
    }

    const selectedTemplate = document.querySelector(".template-card.student.active") as HTMLDivElement;
    const studentId = parseInt(selectedTemplate.dataset.id ?? "2");

    let response : IdResponse;

    try {
        response = await ResHandler.IdGeneratorResponse("student", studentName.value, studentEmail.value, studentCourse.value, 
        studentSchool.value, studentId, studentImage.files[0]!);
    } catch (error){
        console.log(error)
        formSubmitBtn.forEach(btn => btn.disabled = false);
        return;
    }

    sessionStorage.setItem("result", JSON.stringify(response));
    formSubmitBtn.forEach(btn => btn.disabled = false);
    window.location.href = "./preview.html"
})


employeeForm.addEventListener("submit", async (e) => {
    e.preventDefault();
    formSubmitBtn.forEach(btn => btn.disabled = true);

    const employeeName = document.getElementById("employee-name") as HTMLInputElement;
    const employeePosition = document.getElementById("employee-position") as HTMLInputElement;
    const employeeEmail = document.getElementById("employee-email") as HTMLInputElement;
    const employeeCompany = document.getElementById("employee-company") as HTMLInputElement;
    const employeeImage = document.getElementById("employee-image") as HTMLInputElement;

    if (employeeImage.files === null || employeeImage.files.length === 0){
        showToast({
            type: "warning",
            message : "Image must not be empty",
            duration : 3000
        });
        formSubmitBtn.forEach(btn => btn.disabled = false);
        return;
    }

    const selectedTemplate = document.querySelector(".template-card.employee.active") as HTMLDivElement;
    const employeeId = parseInt(selectedTemplate.dataset.id ?? "2");
    let response : IdResponse; 

    try{
        response = await ResHandler.IdGeneratorResponse("employee", employeeName.value, employeeEmail.value, 
            employeePosition.value, employeeCompany.value, employeeId, employeeImage.files[0]!); 
    } catch (error){
        console.log(error)
        formSubmitBtn.forEach(btn => btn.disabled = false);
        return;
    }

    sessionStorage.setItem("result", JSON.stringify(response));
    formSubmitBtn.forEach(btn => btn.disabled = false);
    window.location.href = "./preview.html"
})

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