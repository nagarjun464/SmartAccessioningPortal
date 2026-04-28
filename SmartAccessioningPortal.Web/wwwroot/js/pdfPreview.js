window.pdfPreview = {
    createUrl: function (inputId) {
        const input = document.getElementById(inputId);

        if (!input || !input.files || input.files.length === 0) {
            return null;
        }

        const file = input.files[0];

        if (file.type !== "application/pdf") {
            return null;
        }

        return URL.createObjectURL(file);
    },

    revokeUrl: function (url) {
        if (url) {
            URL.revokeObjectURL(url);
        }
    }
};