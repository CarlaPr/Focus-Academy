document.addEventListener("DOMContentLoaded", function() {
    const form = document.querySelector("form");
    const inputs = form.querySelectorAll("input");

    form.addEventListener("submit", function (event) {
        let isValid = true;

        const errorMessages = form.querySelectorAll(".error-message");
        errorMessages.forEach(msg => msg.remove());

        inputs.forEach(input => {
            const value = input.value.trim();
            const id = input.id;
            let message = "";

            if (input.hasAttribute("required") && !value) {
                isValid = false;
                message = `O campo ${id} é obrigatório.`;
            }

            // Validação do CPF
            if (id === "Cpf" && value && !/^\d{11}$/.test(value)) {
                isValid = false;
                message = "CPF inválido. Deve conter exatamente 11 números.";
            }

            if (id === "DataNascimento" && value) {
                const dataNascimento = new Date(value);
                const anoNascimento = dataNascimento.getFullYear();
                const anoAtual = new Date().getFullYear();
                const passado = 1920;

                if (anoNascimento >= anoAtual) {
                    isValid = false;
                    message = "Data de nascimento inválida. O ano não pode ser igual ou superior ao ano atual.";
                }

                if (anoNascimento <= passado) {
                    isValid = false;
                    message = "Data de nascimento inválida. O ano não pode ser igual ou inferior a 1920.";
                }
            }

            if (id === "Telefone" && value && !/^\d{10,11}$/.test(value)) {
                isValid = false;
                message = "Telefone inválido. Deve conter 10 ou 11 números.";
            }

            if (id === "Email" && value && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value)) {
                isValid = false;
                message = "E-mail inválido. Verifique o formato.";
            }

            if (message) {
                const errorMessage = document.createElement("div");
                errorMessage.classList.add("error-message");
                errorMessage.style.color = "red";
                errorMessage.textContent = message;

                const parent = input.closest(".form-group");
                parent.appendChild(errorMessage);
            }
        });

        if (!isValid) {
            event.preventDefault();
        }
    });
});


document.addEventListener("DOMContentLoaded", function() {
    const form = document.getElementById("cadastroForm");
    const inputs = form.querySelectorAll("input[required]");

    form.addEventListener("submit", function(event) {
        let isValid = true;

        const errorMessages = form.querySelectorAll(".error-message");
        errorMessages.forEach(msg => msg.remove());

        inputs.forEach(input => {
            const value = input.value.trim();
            const id = input.id;
            let message = "";

            if (input.hasAttribute("required") && !value) {
                isValid = false;
                message = `O campo ${id} é obrigatório.`;
            }

            // Validação do CPF
            if (id === "Cpf" && value && !/^\d{11}$/.test(value)) {
                isValid = false;
                message = "CPF inválido. Deve conter exatamente 11 números.";
            }

            if (id === "DataNascimento" && value) {
                const dataNascimento = new Date(value);
                const anoNascimento = dataNascimento.getFullYear();
                const anoAtual = new Date().getFullYear();
                const passado = 1920;

                if (anoNascimento >= anoAtual) {
                    isValid = false;
                    message = "Data de nascimento inválida. O ano não pode ser igual ou superior ao ano atual.";
                }

                if (anoNascimento <= passado) {
                    isValid = false;
                    message = "Data de nascimento inválida. O ano não pode ser igual ou inferior a 1920.";
                }
            }

            if (id === "Telefone" && value && !/^\d{10,11}$/.test(value)) {
                isValid = false;
                message = "Telefone inválido. Deve conter 10 ou 11 números.";
            }

            // Validação do E-mail
            if (id === "Email" && value && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value)) {
                isValid = false;
                message = "E-mail inválido. Verifique o formato.";
            }

            if (message) {
                const errorMessage = document.createElement("div");
                errorMessage.classList.add("error-message");
                errorMessage.style.color = "red";
                errorMessage.textContent = message;

                const parent = input.closest(".form-group");
                parent.appendChild(errorMessage);
            }
        });

        if (!isValid) {
            event.preventDefault();
        }
    });
});
