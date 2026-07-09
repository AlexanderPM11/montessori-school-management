import { useNavigate } from "react-router-dom";

export const BtnsForm = ({
  navigateLink,
}: {
  navigateLink?: string | null;
}) => {
  const navigate = useNavigate();

  return (
    <div className="flex justify-center md:justify-end gap-4 mt-6">
      <button
        type="button"
        onClick={() =>
          navigateLink
            ? navigate(navigateLink, { replace: true })
            : navigate(-1)
        }
        className="w-32 px-4 py-2 bg-gray-300 text-gray-800 rounded-lg hover:bg-gray-400 transition-colors"
      >
        Regresar
      </button>
      <button
        type="submit"
        className="w-32 px-4 py-2 bg-gray-900 text-white rounded-lg hover:bg-gray-800 transition-colors"
      >
        Guardar
      </button>
    </div>
  );
};
