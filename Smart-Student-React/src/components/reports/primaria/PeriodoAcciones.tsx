import React from "react";
import { AiFillEye } from "react-icons/ai";
import { MdOutlineDownload } from "react-icons/md";
import { PeriodoSelector } from "./PeriodoSelector";
import { SelectedInterface } from "../../../interfaces/SelectedInterface";
import { SubjectSelector } from "./SubjectSelector";

type Props = {
  periodSelect: string;
  subjectSelect: number;
  subjects: SelectedInterface<number>[];
  periods: SelectedInterface<string>[];
  onPeriodChange: (next: string) => void;
  onSubjectChange: (next: number) => void;
  onPreview: () => void;
  onDownload: () => void;
};

export const PeriodoAcciones: React.FC<Props> = ({
  periodSelect,
  periods,
  subjectSelect,
  subjects,
  onPeriodChange,
  onSubjectChange,
  onPreview,
  onDownload,
}) => {
  return (
    <div className="flex flex-col sm:flex-row sm:justify-between sm:items-center gap-3 mb-4">
      <div className="w-full sm:w-auto flex gap-2">
        <PeriodoSelector
          value={periodSelect}
          options={periods}
          onChange={onPeriodChange}
        />
        <SubjectSelector
          value={subjectSelect}
          options={subjects}
          onChange={onSubjectChange}
        />
      </div>

      <div className="flex gap-2 flex-wrap justify-end sm:justify-start">
        <button
          onClick={onPreview}
          className="border-gray-900 border bg-gray-900 text-white px-3 py-2 rounded hover:bg-white hover:text-gray-900 flex items-center gap-1 text-sm sm:text-base sm:px-4 flex-1 sm:flex-none min-w-[120px] justify-center transition-colors duration-200"
        >
          <AiFillEye className="text-base" />
          <span>Vista Previa</span>
        </button>
        <button
          onClick={onDownload}
          className="border-gray-900 border text-gray-900 px-3 py-2 rounded hover:bg-gray-900 hover:text-white flex items-center gap-1 text-sm sm:text-base sm:px-4 flex-1 sm:flex-none min-w-[120px] justify-center transition-colors duration-200"
        >
          <MdOutlineDownload className="text-base" />
          <span>Descargar</span>
        </button>
      </div>
    </div>
  );
};
