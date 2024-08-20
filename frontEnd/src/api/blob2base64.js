export function blobToBase64(blob) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.onloadend = () => {
            resolve(reader.result);
        };
        reader.onerror = () => {
            reject(new Error("Failed to convert blob to base64"));
        };
        reader.readAsDataURL(blob);
    });
}