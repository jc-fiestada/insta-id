import { IdResponse } from "../interface/id.response";
import { showToast } from "../ui/toast";
import { saveAs } from 'file-saver';

const pdfViewer = document.getElementById("pdfViewer") as HTMLIFrameElement;
const downloadBtn = document.getElementById("download-btn") as HTMLButtonElement;

document.addEventListener("DOMContentLoaded", () => {
    const rawData = sessionStorage.getItem("result");
    
    if (rawData === null) {
        window.location.href = "./main.html";
        return;
    }

    const data: IdResponse = JSON.parse(rawData);

    if (data.pdfStatusCode !== 200) {
        window.location.href = "./main.html";
        return;
    }

    const binaryString = atob(data.pdfBase64);
    const bytes = new Uint8Array(binaryString.length);
    for (let i = 0; i < binaryString.length; i++) {
        bytes[i] = binaryString.charCodeAt(i);
    }
    const pdfBlob = new Blob([bytes], { type: 'application/pdf' });

    const blobUrl = URL.createObjectURL(pdfBlob);
    pdfViewer.src = blobUrl;

    downloadBtn?.addEventListener("click", () => {
        saveAs(pdfBlob, "id-card.pdf");
    });

    showToast({
        type: "success",
        message: "Id has been successfully generated",
        duration: 3000
    });
});