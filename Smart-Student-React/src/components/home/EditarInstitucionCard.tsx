
import { FaEdit, FaTimes } from "react-icons/fa";
import { EditCreateInstituCenter } from "../../Formik/InstituCenter/EditCreateInstituCenter";

interface Props {
  onCancel: () => void;
}

export const EditarInstitucionCard = ({ onCancel }: Props) => {
  return (
    <div className="bg-white rounded-2xl shadow-xl overflow-hidden">
      <div className="p-8">
        <div className="flex justify-between items-center mb-8 border-b pb-6">
          <div>
            <h2 className="text-3xl font-bold text-gray-800 flex items-center">
              <FaEdit className="mr-3 text-blue-600" />
              Editar Institución
            </h2>
            <p className="text-gray-500 mt-1">
              Actualiza los detalles de la institución a continuación
            </p>
          </div>
          <button
            onClick={onCancel}
            className="text-gray-400 hover:text-gray-600 transition-colors p-2 rounded-full hover:bg-gray-100"
          >
            <FaTimes size={24} />
          </button>
        </div>
        <EditCreateInstituCenter onSave={onCancel} />
      </div>
    </div>
  );
};
