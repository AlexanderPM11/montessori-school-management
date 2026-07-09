import { useEffect, useState, useRef } from "react";
import { useReportDataStore } from "../../../hooks/store/Reports/Reports.store";

interface Props {
  idStudent: number;
  period: string;
}

export const CommentSection = ({ idStudent, period }: Props) => {
  const OnSaveComments = useReportDataStore((state) => state.OnSaveComments);
  const reportData = useReportDataStore((state) => state.reportData);

  const [comment1, setComment1] = useState(reportData.comment1 ?? "");
  const [comment2, setComment2] = useState(reportData.comment2 ?? "");

  // Ref para controlar si es la primera renderización
  const isFirstRender = useRef(true);

  useEffect(() => {
    // Saltar la primera ejecución del useEffect
    if (isFirstRender.current) {
      isFirstRender.current = false;
      return;
    }

    const timeout = setTimeout(() => {
      OnSaveComments({
        idStudent,
        comment1,
        comment2,
        period,
      });
    }, 800);

    return () => clearTimeout(timeout);
  }, [OnSaveComments, comment1, comment2, idStudent, period]);

  return (
    <div className="mt-6 space-y-4">
      <h2 className="text-xl font-semibold mb-2">
        Observaciones y Comentarios:
      </h2>

      <div>
        <label htmlFor="comentarioDocente" className="font-medium block mb-1">
          Cualidades y habilidades a destacar del niño/niña:
        </label>
        <textarea
          id="comentarioDocente"
          name="comentarioDocente"
          rows={5}
          placeholder="Escriba un comentario..."
          className="w-full border border-gray-300 rounded p-2 focus:outline-none focus:ring-2 focus:ring-gray-900"
          value={comment1}
          onChange={(e) => setComment1(e.target.value)}
        />
      </div>

      <div>
        <label
          htmlFor="comentarioInstitucion"
          className="font-medium block mb-1"
        >
          Aspectos en los que necesita apoyo:
        </label>
        <textarea
          id="comentarioInstitucion"
          name="comentarioInstitucion"
          rows={5}
          placeholder="Escriba un comentario..."
          className="w-full border border-gray-300 rounded p-2 focus:outline-none focus:ring-2 focus:ring-gray-900"
          value={comment2}
          onChange={(e) => setComment2(e.target.value)}
        />
      </div>
    </div>
  );
};
