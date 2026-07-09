import { FaChevronDown } from "react-icons/fa";
import { useReportDataStore } from "../../../hooks/store/Reports/Reports.store";
import { useState } from "react";
import { EvaluacionItem } from "./EvaluacionItem";
interface Props {
  idStudent: number;
  period: string;
  subject: number;
}
export const EvaluationList: React.FC<Props> = ({
  idStudent,
  period,
  subject,
}) => {
  const reportData = useReportDataStore((state) => state.reportData);
  const OnUpdateCalification = useReportDataStore(
    (state) => state.OnUpdateCalification
  );
  const OnDeleteCalification = useReportDataStore(
    (state) => state.OnDeleteCalification
  );

  const [expanded, setExpanded] = useState(true);
  const toggle = () => setExpanded((prev) => !prev);

  return (
    <div className="accordion-item border rounded-lg mb-4">
      <h2 className="accordion-header text-[14px] md:text-[16px]">
        <button
          className="accordion-button w-full flex justify-between items-center text-left py-3 px-4 font-medium"
          type="button"
          onClick={toggle}
        >
          Competencias Fundamentales y Específicas del Grado
          <FaChevronDown
            className={`transition-transform duration-300 ${
              expanded ? "rotate-180" : ""
            }`}
          />
        </button>
      </h2>

      <div
        className={`accordion-collapse transition-all duration-300 ease-in-out ${
          expanded ? "block" : "hidden"
        }`}
      >
        <div className="evaluation p-4 space-y-3">
          {/* Evaluaciones ya existentes */}
          {reportData.vmPrinc.map((item) => (
            <EvaluacionItem
              key={item.indicator.id}
              indicator={item.indicator}
              calification={item.calification}
              onEliminar={() => {
                const requestReport = {
                  idStudent,
                  idAchievementIndicator: item.indicator.id,
                  period: period,
                  score: 0,
                  estado: "",
                  idSubject: subject,
                };
                OnDeleteCalification(requestReport);
              }}
              onChangeEstado={(value) => {
                const requestReport = {
                  idStudent,
                  idAchievementIndicator: item.indicator.id,
                  period: period,
                  score: Number(value),
                  estado: "",
                  idSubject: subject,
                };
                OnUpdateCalification(requestReport);
              }}
            />
          ))}
        </div>
      </div>
    </div>
  );
};
