import Swal from "sweetalert2";

interface Props {
  title?: string;
  confirmButtonText?: string;
  cancelButtonText?: string;
  onSubmit: (email: string) => void;
}

export const SweetAlertChangePassword = async ({
  title = "Cambiar contraseña",
  confirmButtonText = "Enviar",
  cancelButtonText = "Cancelar",
  onSubmit,
}: Props) => {
  const { value: email } = await Swal.fire({
    title,
    input: "email",
    inputLabel: "Correo electrónico",
    inputPlaceholder: "ejemplo@correo.com",
    inputAttributes: {
      autocapitalize: "off",
      autocorrect: "off",
    },
    showCancelButton: true,
    confirmButtonText,
    cancelButtonText,
    confirmButtonColor: "rgb(31 41 55)",
    preConfirm: (value) => {
      if (!value) {
        Swal.showValidationMessage("Por favor, ingresa tu correo electrónico.");
        return false;
      }

      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
      if (!emailRegex.test(value)) {
        Swal.showValidationMessage("El formato del correo no es válido.");
        return false;
      }

      return value;
    },
  });

  if (email) {
    onSubmit(email);
  }
};
