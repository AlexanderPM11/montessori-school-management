import { CustomSelect } from "../../CustomSelect";

interface SubjectSelectorSelectorProps {
  value: number;
  onChange: (next: number) => void;
  options: { text: string; id: number }[];
}

export const SubjectSelector: React.FC<SubjectSelectorSelectorProps> = ({
  value,
  onChange,
  options,
}) => (
  <CustomSelect
    label="Asignatura"
    name="asignatura"
    value={value}
    onChange={(_, next) => onChange(next)}
    options={options.map((opt) => ({
      label: opt.text,
      value: opt.id,
    }))}
  />
);
