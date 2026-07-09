import * as Yup from "yup";

import { Formik, Form } from "formik";

import { CustomInput } from "../CustomInput";
import { useState } from "react";
import {
  closeCustomLoading,
  showCustomLoading,
} from "../../util/showCustomLoading";

interface Props {
  search: string;
  // Functions
  onChangeCustom?: (value: string) => void;
}

export const SearchForm = ({ onChangeCustom, search }: Props) => {
  const [isLoading, setLoading] = useState(false);

  const validationSchema = Yup.object({
    // search: Yup.string()
    //     .required('Requerido')
  });
  if (isLoading) {
    showCustomLoading();
  } else {
    closeCustomLoading();
  }

  return (
    <Formik
      initialValues={{
        search,
      }}
      validationSchema={validationSchema}
      onSubmit={async () => {
        setLoading(true);
        setLoading(false);
      }}
    >
      {({ handleSubmit }) => (
        <div>
          <Form onSubmit={handleSubmit} noValidate>
            <CustomInput
              py="3"
              name="search"
              placeholder="Buscar"
              type="text"
              value={search}
              onChangeCustomVoid={onChangeCustom}
            />
          </Form>
        </div>
      )}
    </Formik>
  );
};
