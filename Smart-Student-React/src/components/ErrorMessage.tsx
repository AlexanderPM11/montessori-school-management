import { useEffect } from "react";
import Swal from "sweetalert2";

interface Props {
  message: string;
}

const ErrorMessageSwal: React.FC<Props> = ({ message }) => {
  useEffect(() => {
    Swal.fire({
      icon: "error",
      title: "¡Error!",
      text: message,
      confirmButtonColor: "#d33",
    });
  }, [message]);

  return null; // No renderiza nada
};

export default ErrorMessageSwal;
