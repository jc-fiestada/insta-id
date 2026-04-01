import { IdResponse } from "../interface/id.response";
import { showToast } from "../ui/toast";

const pdfViewer = document.getElementById("pdfViewer") as HTMLIFrameElement;
const downloadBtn = document.getElementById("download-btn") as HTMLButtonElement;

document.addEventListener("DOMContentLoaded", () => {
    const rawData = sessionStorage.getItem("result");
    if (rawData === null){
        window.location.href = "./main.html";
        return;
    }

    const data : IdResponse = JSON.parse(rawData);

    if (data.pdfStatusCode !== 200) {
        window.location.href = "./main.html";
        return;
    }

    pdfViewer.src = `data:application/pdf;base64,${data.pdfBase64}`

    downloadBtn?.addEventListener("click", () => {
        const link = document.createElement('a');
        link.href = `data:application/pdf;base64,${data.pdfBase64}`;
        link.download = "id-card.pdf";
        link.click();
    });

    if (data.gmailStatusCode !== 200){
        showToast({
            type: "error",
            message : data.message,
            duration : 3000
        });
        return;
    }

    showToast({
        type: "success",
        message : "Id has been successfully sent via Gmail",
        duration : 3000
    });
});