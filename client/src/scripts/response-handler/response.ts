import { IdResponse } from "../interface/id.response";
import { showToast} from "../ui/toast";

export class ResponseHandler{

    public async IdGeneratorResponse(entityType: string, name: string, gmail: string, role: string, institute: string, id: number, imgFile : File) : Promise<IdResponse>{

        const payload = {
            "name" : name,
            "gmail" : gmail,
            "role" : role,
            "institute" : institute,
            "entityType" : entityType,
            "id" : id
        }

        const form = new FormData();

        form.append("entity", JSON.stringify(payload));
        form.append("img", imgFile)

        const response = await fetch("/generate-id", {
            method : "POST",
            body : form
        })

        if (!response.ok){
            const errorMessage = await response.text();
            showToast({
                type: "error",
                title: "Id Generation Failed",
                message : errorMessage,
                duration : 3000
            });
            throw new Error(errorMessage);
        }

        const data : IdResponse = await response.json();
        return data;
    }
}
