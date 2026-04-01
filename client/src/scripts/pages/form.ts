import { ResponseHandler } from "../response-handler/response";
import { showToast } from "../ui/toast";
import { IdResponse } from "../interface/id.response";
import { InputValidator } from "../tools/input.validator";

// Handlers/Helpers
const resHandler : ResponseHandler = new ResponseHandler(); 
const Validator : InputValidator = new InputValidator();

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

    const selectedTemplate = document.querySelector(".template-card.student.active") as HTMLDivElement;
    const studentId = parseInt(selectedTemplate.dataset.id ?? "2");

    // validate user input first
    try {
        Validator.ValidateStudent(studentName.value, studentEmail.value, studentSchool.value, studentCourse.value, studentImage, studentId);
    } catch (error){
        if (error instanceof Error){
            showToast({
                type: "warning",
                message : error.message,
                duration : 3000
            });
            formSubmitBtn.forEach(btn => btn.disabled = false);
            return;
        }
    }

    let response : IdResponse;

    try {
        response = await resHandler.IdGeneratorResponse("student", studentName.value, studentEmail.value, studentCourse.value, 
        studentSchool.value, studentId, studentImage.files![0]!);
    } catch (error){
        if (error instanceof Error) console.log(error.message);
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

    const selectedTemplate = document.querySelector(".template-card.employee.active") as HTMLDivElement;
    const employeeId = parseInt(selectedTemplate.dataset.id ?? "2");

    try {
        Validator.ValidateEmployee(employeeName.value, employeeEmail.value, employeeCompany.value, employeePosition.value, employeeImage, employeeId);
    } catch (error){
        if (error instanceof Error){
            showToast({
                type: "warning",
                message : error.message,
                duration : 3000
            });
            formSubmitBtn.forEach(btn => btn.disabled = false);
            return;
        }
    }

    let response : IdResponse; 

    try{
        showToast({
            type: "success",
            message : "Your information is being processed. Please wait.",
            duration : 3000
        });
        response = await resHandler.IdGeneratorResponse("employee", employeeName.value, employeeEmail.value, 
            employeePosition.value, employeeCompany.value, employeeId, employeeImage.files![0]!); 
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