-- Categorías de Eventos
INSERT INTO CategoriasEventos (idCatEvento, nombre) VALUES
(1, 'Taller'),
(2, 'Seminario'),
(3, 'Conferencia');

-- Eventos
INSERT INTO Eventos (idEvento, idCatEvento, nombre, totalHoras) VALUES
(1, 1, 'Taller de Python', 4),
(2, 2, 'Seminario de Ciberseguridad', 6),
(3, 3, 'Conferencia de Inteligencia Artificial', 8);

-- Lugares
INSERT INTO Lugares (idLugar, nombre, capacidad) VALUES
(1, 'Auditorio Principal', 100),
(2, 'Sala 101', 40),
(3, 'Sala Virtual Zoom', 200);

-- Reuniones
INSERT INTO Reuniones (idReunion, idEvento, idLugar, horario) VALUES
(1, 1, 1, '2025-06-15 09:00'),
(2, 2, 2, '2025-06-20 14:00'),
(3, 3, 3, '2025-06-25 10:00');

-- Participantes
INSERT INTO Participantes (idParticipante, nombre, apellido, mail, dni, contraseña) VALUES
(1, 'Ana', 'López', 'ana@example.com', '12345678', 'pass123'),
(2, 'Carlos', 'Pérez', 'carlos@example.com', '23456789', 'pass456'),
(3, 'María', 'García', 'maria@example.com', '34567890', 'pass789');

-- Inscripciones
INSERT INTO Inscripciones (idEvento, idParticipante) VALUES
(1, 1),
(2, 1),
(2, 2),
(3, 2),
(3, 3);

-- Asistencias
INSERT INTO Asistencias (idEvento, idParticipante, horariosAsistido) VALUES
(1, 1, '2025-06-15 09:00-13:00'),
(2, 1, '2025-06-20 14:00-18:00'),
(2, 2, '2025-06-20 14:15-18:00'),
(3, 3, '2025-06-25 10:00-18:00');

-- Categorías de Directivos
INSERT INTO CategoriasDirectivos (idCatDirectiva, nombre) VALUES
(1, 'Coordinador'),
(2, 'Organizador'),
(3, 'Moderador');

-- Directivos
INSERT INTO Directivos (idReunion, idParticipante, idCatDirectiva) VALUES
(1, 1, 1), -- Ana es Coordinadora en la reunión 1
(2, 2, 2), -- Carlos es Organizador en la reunión 2
(3, 3, 3); -- María es Moderadora en la reunión 3