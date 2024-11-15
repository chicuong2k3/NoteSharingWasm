
function downloadFile(fileName, base64Content) {
    const link = document.createElement('a');
    link.href = 'data:text/markdown;base64,' + base64Content;
    link.download = fileName;
    link.click();
}