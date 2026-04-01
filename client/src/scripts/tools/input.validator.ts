import { showToast } from "../ui/toast";

export class InputValidator{
    private validateName(name: string): void {
        if (!name || name.trim() === "") {
            throw new Error("Name must not be empty");
        }
        if (name.length > 20) {
            throw new Error("Name must not exceed more than 20 Characters");
        }
        if (!/^[A-Za-z ]+$/.test(name)) {
            throw new Error("Name must only include letters and spaces");
        }
    }

    private validateGmail(gmail: string): void {
        if (!gmail || gmail.trim() === "") {
            throw new Error("Gmail must not be empty");
        }
        if (gmail.length > 40) {
            throw new Error("Gmail must not exceed more than 40 characters");
        }
        if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(gmail)) {
            throw new Error("Invalid Gmail Format");
        }
    }

    private validateInstitution(institute: string, entityType: string, entityAttr: string): void {
        if (!institute || institute.trim() === "") {
            throw new Error(`${entityAttr} must not be empty`);
        }
        if (institute.length > 25) {
            throw new Error(`${entityType} ${entityAttr} must not exceed more than 25 Characters`);
        }
        if (!/^[A-Za-z ]+$/.test(institute)) {
            throw new Error(`${entityType} ${entityAttr} must only include letters and spaces`);
        }
    }

    private validateRole(role: string, entityType: string, entityAttr: string): void {
        if (!role || role.trim() === "") {
            throw new Error(`${entityAttr} must not be empty`);
        }
        if (role.length > 20) {
            throw new Error(`${entityType} ${entityAttr} must not exceed more than 20 Characters`);
        }
        if (!/^[A-Za-z ]+$/.test(role)) {
            throw new Error(`${entityType} ${entityAttr} must only include letters and spaces`);
        }
    }

    private validateImage(studentImage : HTMLInputElement){
        if (studentImage.files === null || studentImage.files.length === 0){
            throw new Error("Image must not be empty");
        }
    }

    private validateSelectedTemplate(id: number): void {
        if (id === null || id === undefined) {
            throw new Error("Selected id field is missing");
        }
        if (id !== 1 && id !== 2 && id !== 3) {
            throw new Error("Invalid template choice");
        }
    }

    public ValidateStudent(name: string, gmail: string, school: string, course: string, image: HTMLInputElement, id: number) : void{
        const entityType : string = "Student"; 

        this.validateName(name);
        this.validateRole(course, entityType, "Course");
        this.validateGmail(gmail);
        this.validateInstitution(school, entityType, "School name");
        this.validateImage(image);
        this.validateSelectedTemplate(id);
    }

    public ValidateEmployee(name: string, gmail: string, company: string, position: string, image: HTMLInputElement, id: number) : void{
        const entityType : string = "Employee"; 

        this.validateName(name);
        this.validateRole(position, entityType, "Position");
        this.validateGmail(gmail);
        this.validateInstitution(company, entityType, "Company name");
        this.validateImage(image);
        this.validateSelectedTemplate(id);
    }
}