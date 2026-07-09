import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

interface Props {
  type: 'success' | 'error' | 'warning' | 'info' | 'default';
  message: string;
  options?: object; // Opcional, para configuraciones adicionales de Toastify
}

export const ToastifyCustom = ({ type, message, options = {
  autoClose: 10000,
  position: "top-right",
} }: Props) => {
  switch (type) {
    case "success":
      toast.success(message, options);
      break;
    case "error":
      toast.error(message, options);
      break;
    case "warning":
      toast.warn(message, options);
      break;
    case "info":
      toast.info(message, options);
      break;
    default:
      toast(message, options); // Tipo "default"
      break;
  }
};
