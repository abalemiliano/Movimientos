# Movimientos

Una aplicación móvil diseñada para el control diario de ingresos y egresos personales. El objetivo principal del proyecto fue crear una herramienta fluida y totalmente funcional offline, aplicando buenas prácticas de arquitectura de software y optimización de recursos.

---

## Capturas de Pantalla

| Pantalla Principal Dashboard | Historial de Movimientos y Filtros | Alta de Rubro |
| :---: | :---: | :---: |
| !(https://drive.google.com/file/d/1M9FoUW3n5ux7yLcWhW_XS4XqlvmPPvFD/view?usp=sharing) | !(https://drive.google.com/file/d/1KlPWFpRRnoVZ0V8T2TZXxBa34nbBzFOI/view?usp=sharing) | !(https://drive.google.com/file/d/1Al70G7aWpaFqazWzNO_hXkEJNhX9t9h-/view?usp=sharing) |

---

## Características Principales

* **Registro de Transacciones:** Gestión de ingresos y egresos categorizados por rubros personalizados.
* **Filtrado y Paginación Optimizada:** Listado de movimientos con soporte para filtrado dinámico (por mes, año y rubro) realizando consultas directamente en base de datos local.
* **Interfaz Intuitiva:** Indicadores gráficos visuales mediante código de colores.
* **Offline:** Persistencia de datos local sin depender de conectividad constante.

---

## Stack Tecnológico y Arquitectura

* **Lenguaje:** C# (.NET 10)
* **Framework Móvil:** .NET MAUI
* **Patrón de Diseño:** MVVM (Model-View-ViewModel) con `CommunityToolkit.Mvvm`
* **Persistencia Local:** SQLite (`sqlite-net-pcl`)
* **Consultas:** LINQ optimizado para paginación y filtrado en BD
