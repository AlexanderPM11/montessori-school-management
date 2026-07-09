interface BtnsFormProps {
  onClose: () => void;
}
export const BtnsForm = ({ onClose }: BtnsFormProps) => {
  return (
    <div className="flex justify-center md:justify-end gap-4 mt-6">
      <button
        type="button"
        onClick={() => onClose()}
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
