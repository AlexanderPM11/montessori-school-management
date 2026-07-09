import { useState } from "react";
import { useReportDataStore } from "../../../hooks/store/Reports/Reports.store";
import { EvaluacionItem } from "../EvaluacionItem";
import { FaChevronDown } from "react-icons/fa";

interface Props {
  idStudent: number;
  period: string;
}

export const EvaluationList: React.FC<Props> = ({ idStudent, period }) => {
  const reportData = useReportDataStore((state) => state.reportData);
  const updateEstadoVmPrinc = useReportDataStore(
    (state) => state.updateEstadoVmPrinc
  );
  const clearEstadoVmPrinc = useReportDataStore(
    (state) => state.clearEstadoVmPrinc
  );
  const OnUpdateEvaluation = useReportDataStore(
    (state) => state.OnUpdateEvaluation
  );
  const OnDeleteEvaluation = useReportDataStore(
    (state) => state.OnDeleteEvaluation
  );
  const [expandedItems, setExpandedItems] = useState<{
    [key: string]: boolean;
  }>({});

  const toggle = (key: string) => {
    setExpandedItems((prev) => ({
      ...prev,
      [key]: !prev[key],
    }));
  };

  return (
    <>
      {reportData.evaluationsDetails.map(
        ({ id, title, subTitle, indicators }) => {
          const isExpanded = expandedItems[id] ?? false;
          return (
            <div key={id} className="accordion-item border rounded-lg mb-4">
              <h2 className="accordion-header text-[14px] md:text-[16px]">
                <button
                  className="accordion-button w-full flex justify-between items-center text-left py-3 px-4 font-medium"
                  type="button"
                  onClick={() => toggle(id)}
                  aria-expanded={isExpanded}
                  aria-controls={`collapse-${id}`}
                >
                  <div className="max-w-[270px] md:max-w-full">
                    {title}
                    <br />
                    <span className="text-sm font-normal">{subTitle}</span>
                  </div>
                  <FaChevronDown
                    className={`transition-transform duration-300 ${
                      isExpanded ? "rotate-180" : ""
                    }`}
                  />
                </button>
              </h2>

              <div
                id={`collapse-${id}`}
                className={`accordion-collapse transition-all duration-300 ease-in-out ${
                  isExpanded ? "block" : "hidden"
                }`}
              >
                <div className="evaluation p-4 space-y-3">
                  {indicators.map((item) => (
                    <EvaluacionItem
                      key={item.indicator.id}
                      indicator={item.indicator}
                      estado={item.estado}
                      onEliminar={() => {
                        clearEstadoVmPrinc(item.indicator.id);
                        OnDeleteEvaluation({
                          idStudent,
                          idAchievementIndicator: item.indicator.id,
                          period,
                          estado: "",
                          idSubject: 0,
                        });
                      }}
                      onChangeEstado={(newEstado) => {
                        updateEstadoVmPrinc(item.indicator.id, newEstado);
                        OnUpdateEvaluation({
                          idStudent,
                          idAchievementIndicator: item.indicator.id,
                          period,
                          estado: newEstado,
                          idSubject: 0,
                        });
                      }}
                    />
                  ))}
                </div>
              </div>
            </div>
          );
        }
      )}
    </>
  );
};
