import { FC, useEffect, useRef, useState } from "react";

interface DebouncedInputProps {
  value: string | number;
  onChange: (value: string | number) => void;
  debounce?: number;
  className?: string;
  placeholder?: string;
  type?: string;
}
export const DebouncedInput: FC<DebouncedInputProps> = ({
  value: initialValue,
  onChange,
  debounce = 500,
  className = "",
  placeholder = "",
  type = "text",
  ...props
}) => {
  const [value, setValue] = useState(initialValue);
  const isFirstRun = useRef(true);

  useEffect(() => {
    setValue(initialValue);
  }, [initialValue]);

  useEffect(() => {
    if (isFirstRun.current) {
      isFirstRun.current = false;
      return;
    }

    const timeout = setTimeout(() => {
      onChange(value);
    }, debounce);

    return () => clearTimeout(timeout);
  }, [value, debounce, onChange]);

  return (
    <input
      type={type}
      value={value}
      onChange={(e) => setValue(e.target.value)}
      className={`${className} transition-all`}
      placeholder={placeholder}
      {...props}
    />
  );
};
