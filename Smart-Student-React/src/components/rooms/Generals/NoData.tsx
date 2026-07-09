interface NotFoundFilterProps {
  title?: string;
  message?: string;
}
export const NotFoundData = ({ message, title }: NotFoundFilterProps) => {
  return (
    <div className="flex-1 flex items-center justify-center">
      <div className="flex flex-col items-center justify-center space-y-3 p-12">
        <h2 className="text-xl font-semibold text-gray-700">
          {title ?? "No se encontraron resultados"}
        </h2>
        <p className="text-gray-500">
          {message ??
            "Intenta ajustar tus filtros o buscar con otros términos."}
        </p>
      </div>
    </div>
  );
};
