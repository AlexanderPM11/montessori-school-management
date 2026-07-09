import { CustomSelect } from "../../CustomSelect";

interface PeriodoSelectorProps {
  value: string;
  onChange: (next: string) => void;
  options: { text: string }[];
}

export const PeriodoSelector: React.FC<PeriodoSelectorProps> = ({
  value,
  onChange,
  options,
}) => (
  <CustomSelect
    label="Periodo"
    name="periodo"
    value={value}
    onChange={(_, next) => onChange(next)}
    options={options.map((opt) => ({
      label: opt.text,
      value: opt.text,
    }))}
  />
);
