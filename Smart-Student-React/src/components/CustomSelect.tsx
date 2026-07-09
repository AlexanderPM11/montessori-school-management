interface Option<T> {
  label: string;
  value: T;
}

interface CustomSelectProps<T> {
  label: string;
  name: string;
  value: T;
  options: Option<T>[];
  onChange: (previousValue: T, newValue: T) => void;
  required?: boolean;
  className?: string;
}

export function CustomSelect<T extends string | number>({
  label,
  name,
  value,
  options,
  onChange,
  required = false,
  className = "",
}: CustomSelectProps<T>) {
  return (
    <div className={`flex flex-col gap-1 ${className}`}>
      <label htmlFor={name} className="text-sm font-medium text-gray-700">
        {label} {required && <span className="text-red-500">*</span>}
      </label>
      <div className="relative">
        <select
          id={name}
          name={name}
          value={value}
          required={required}
          onChange={(e) => onChange(value, e.target.value as T)}
          className="
          w-full
          border border-gray-300 
          rounded-lg 
          px-3 py-3 pr-10
          bg-white text-gray-800 
          focus:outline-none 
          appearance-none
          transition duration-150 ease-in-out"
        >
          {options.map((opt) => (
            <option key={opt.value} value={opt.value}>
              {opt.label}
            </option>
          ))}
        </select>
        <div className="absolute inset-y-0 right-3 flex items-center pointer-events-none">
          <svg
            className="w-4 h-4 text-gray-500"
            fill="none"
            stroke="currentColor"
            strokeWidth="2"
            viewBox="0 0 24 24"
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              d="M19 9l-7 7-7-7"
            />
          </svg>
        </div>
      </div>
    </div>
  );
}
