
import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  Tooltip,
  ResponsiveContainer,
  PieChart,
  Pie,
  Cell,
  LineChart,
  Line,
  Legend,
  CartesianGrid,
} from "recharts";
import {
  FiUsers,
  FiBook,
  FiAward,
  FiFileText,
  FiTrendingUp,
} from "react-icons/fi";

const COLORS = [
  "#34D399",
  "#60A5FA",
  "#FBBF24",
  "#F87171",
  "#A78BFA",
  "#818CF8",
];

// Simulación de datos
const usuariosPorRol = [
  { name: "Estudiantes", value: 420 },
  { name: "Profesores", value: 35 },
  { name: "Padres", value: 280 },
  { name: "Secretarios", value: 50 },
  { name: "Administradores", value: 10 },
];

const asistenciaPorCurso = [
  { curso: "1ro A", asistieron: 25, ausentes: 5 },
  { curso: "2do B", asistieron: 22, ausentes: 8 },
  { curso: "3ro C", asistieron: 28, ausentes: 2 },
  { curso: "4to A", asistieron: 30, ausentes: 0 },
];

const promedioPorCurso = [
  { curso: "1ro A", promedio: 82, meta: 80 },
  { curso: "2do B", promedio: 75, meta: 80 },
  { curso: "3ro C", promedio: 89, meta: 85 },
  { curso: "4to A", promedio: 70, meta: 75 },
];

const reportesGenerados = [
  { mes: "Ene", total: 22 },
  { mes: "Feb", total: 35 },
  { mes: "Mar", total: 18 },
  { mes: "Abr", total: 40 },
  { mes: "May", total: 55 },
];

// Quick stats data
const quickStats = [
  {
    title: "Total Estudiantes",
    value: "743",
    change: "+12%",
    trend: "up",
    icon: <FiUsers className="text-blue-500" size={24} />,
  },
  {
    title: "Asistencia Hoy",
    value: "92%",
    change: "+3%",
    trend: "up",
    icon: <FiBook className="text-green-500" size={24} />,
  },
  {
    title: "Promedio General",
    value: "81.5",
    change: "+2.3",
    trend: "up",
    icon: <FiAward className="text-yellow-500" size={24} />,
  },
  {
    title: "Reportes Mes",
    value: "55",
    change: "+15",
    trend: "up",
    icon: <FiFileText className="text-purple-500" size={24} />,
  },
];

export const DashboardPage = () => {
  return (
    <div className="p-4 md:p-6 space-y-6 bg-gray-50 min-h-screen">
      {/* Header */}
      <div className="flex flex-col md:flex-row justify-between items-start md:items-center">
        <h1 className="text-2xl md:text-3xl font-bold text-gray-800">
          Dashboard Escolar
        </h1>
        <div className="mt-2 md:mt-0 text-sm text-gray-500">
          Última actualización: Hoy a las{" "}
          {new Date().toLocaleTimeString([], {
            hour: "2-digit",
            minute: "2-digit",
          })}
        </div>
      </div>

      {/* Quick Stats */}
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
        {quickStats.map((stat, index) => (
          <div
            key={index}
            className="bg-white p-4 rounded-xl shadow-sm border border-gray-100"
          >
            <div className="flex justify-between items-start">
              <div>
                <p className="text-sm font-medium text-gray-500">
                  {stat.title}
                </p>
                <p className="text-2xl font-bold mt-1 text-gray-800">
                  {stat.value}
                </p>
              </div>
              <div className="p-2 bg-gray-50 rounded-lg">{stat.icon}</div>
            </div>
            <div
              className={`flex items-center mt-3 text-sm ${
                stat.trend === "up" ? "text-green-500" : "text-red-500"
              }`}
            >
              <FiTrendingUp className="mr-1" />
              <span>{stat.change}</span>
              <span className="ml-1 text-gray-400">vs mes pasado</span>
            </div>
          </div>
        ))}
      </div>

      {/* Main Charts Grid */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        {/* Distribución por rol */}
        <div className="bg-white p-4 rounded-xl shadow-sm border border-gray-100">
          <div className="flex justify-between items-center mb-4">
            <h2 className="text-lg font-semibold text-gray-800">
              Distribución de usuarios por rol
            </h2>
            <select className="text-sm border border-gray-200 rounded-lg px-3 py-1 bg-gray-50 focus:outline-none focus:ring-2 focus:ring-blue-500">
              <option>Este año</option>
              <option>Mes pasado</option>
            </select>
          </div>
          <div className="h-[300px]">
            <ResponsiveContainer width="100%" height="100%">
              <PieChart data={usuariosPorRol} {...({} as any)}>
                <Pie
                  data={usuariosPorRol}
                  dataKey="value"
                  nameKey="name"
                  cx="50%"
                  cy="50%"
                  outerRadius={100}
                  innerRadius={60}
                  label={({ name, percent }) =>
                    `${name}: ${(percent ?? 1 * 100).toFixed(0)}%`
                  }
                  labelLine={false}
                >
                  {usuariosPorRol.map((_, index) => (
                    <Cell
                      key={`cell-${index}`}
                      fill={COLORS[index % COLORS.length]}
                    />
                  ))}
                </Pie>
                <Tooltip
                  formatter={(value) => [`${value} usuarios`, "Total"]}
                  contentStyle={{
                    borderRadius: "8px",
                    border: "none",
                    boxShadow: "0 2px 10px rgba(0,0,0,0.1)",
                  }}
                />
                <Legend
                  layout="horizontal"
                  verticalAlign="bottom"
                  align="center"
                  wrapperStyle={{ paddingTop: "20px" }}
                />
              </PieChart>
            </ResponsiveContainer>
          </div>
        </div>

        {/* Asistencia por curso */}
        <div className="bg-white p-4 rounded-xl shadow-sm border border-gray-100">
          <h2 className="text-lg font-semibold text-gray-800 mb-4">
            Asistencia de hoy por curso
          </h2>
          <div className="h-[300px]">
            <ResponsiveContainer width="100%" height="100%">
              <BarChart
                {...({} as any)}
                data={asistenciaPorCurso}
                margin={{ top: 20, right: 10, left: 0, bottom: 5 }}
              >
                <CartesianGrid strokeDasharray="3 3" vertical={false} />
                <XAxis dataKey="curso" />
                <YAxis />
                <Tooltip
                  contentStyle={{
                    borderRadius: "8px",
                    border: "none",
                    boxShadow: "0 2px 10px rgba(0,0,0,0.1)",
                  }}
                />
                <Legend />
                <Bar
                  dataKey="asistieron"
                  name="Asistieron"
                  fill="#60A5FA"
                  radius={[6, 6, 0, 0]}
                />
                <Bar
                  dataKey="ausentes"
                  name="Ausentes"
                  fill="#F87171"
                  radius={[6, 6, 0, 0]}
                />
              </BarChart>
            </ResponsiveContainer>
          </div>
        </div>

        {/* Promedio por curso */}
        <div className="bg-white p-4 rounded-xl shadow-sm border border-gray-100 lg:col-span-2">
          <div className="flex justify-between items-center mb-4">
            <h2 className="text-lg font-semibold text-gray-800">
              Promedio de calificaciones por curso
            </h2>
            <div className="flex space-x-2">
              <div className="flex items-center">
                <div className="w-3 h-3 bg-green-400 rounded-full mr-1"></div>
                <span className="text-sm">Promedio</span>
              </div>
              <div className="flex items-center">
                <div className="w-3 h-3 bg-blue-400 rounded-full mr-1"></div>
                <span className="text-sm">Meta</span>
              </div>
            </div>
          </div>
          <div className="h-[300px]">
            <ResponsiveContainer width="100%" height="100%">
              <LineChart
                {...({} as any)}
                data={promedioPorCurso}
                margin={{ top: 20, right: 30, left: 20, bottom: 5 }}
              >
                <CartesianGrid strokeDasharray="3 3" vertical={false} />
                <XAxis dataKey="curso" />
                <YAxis domain={[60, 100]} />
                <Tooltip
                  contentStyle={{
                    borderRadius: "8px",
                    border: "none",
                    boxShadow: "0 2px 10px rgba(0,0,0,0.1)",
                  }}
                />
                <Line
                  type="monotone"
                  dataKey="promedio"
                  name="Promedio"
                  stroke="#34D399"
                  strokeWidth={3}
                  dot={{ r: 6 }}
                  activeDot={{ r: 8 }}
                />
                <Line
                  type="monotone"
                  dataKey="meta"
                  name="Meta"
                  stroke="#60A5FA"
                  strokeWidth={2}
                  strokeDasharray="5 5"
                />
              </LineChart>
            </ResponsiveContainer>
          </div>
        </div>

        {/* Reportes generados */}
        <div className="bg-white p-4 rounded-xl shadow-sm border border-gray-100 lg:col-span-2">
          <h2 className="text-lg font-semibold text-gray-800 mb-4">
            Reportes generados por mes (PDF)
          </h2>
          <div className="h-[300px]">
            <ResponsiveContainer width="100%" height="100%">
              <BarChart
                {...({} as any)}
                data={reportesGenerados}
                margin={{ top: 20, right: 30, left: 20, bottom: 5 }}
              >
                <CartesianGrid strokeDasharray="3 3" vertical={false} />
                <XAxis dataKey="mes" />
                <YAxis />
                <Tooltip
                  contentStyle={{
                    borderRadius: "8px",
                    border: "none",
                    boxShadow: "0 2px 10px rgba(0,0,0,0.1)",
                  }}
                />
                <Bar
                  dataKey="total"
                  name="Reportes"
                  fill="#FBBF24"
                  radius={[6, 6, 0, 0]}
                />
              </BarChart>
            </ResponsiveContainer>
          </div>
        </div>
      </div>
    </div>
  );
};
