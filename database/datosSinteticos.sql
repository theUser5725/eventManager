-- Categoría de Evento
INSERT INTO CategoriasEventos (idCatEvento, nombre)
VALUES (1, 'Evento'),
(2, 'Curso'),
(3, 'Congreso');


-- Evento
INSERT INTO Eventos (idEvento, idCatEvento, nombre, totalHoras, fechaInicio, fechaFinalizacion, estado)
VALUES (1, 1, 'Capacitación de Invierno', 20, '2025-06-21', '2025-07-09', 1);

-- Lugar
INSERT INTO Lugares (idLugar, nombre, capacidad)
VALUES (1, 'Sala de Conferencias A', 100);

-- Participantes
INSERT INTO Participantes (idParticipante, nombre, apellido, mail, dni, contraseña) VALUES
(1, 'Ana', 'Pérez', 'ana.perez@mail.com', '12345678', 'pass123'),
(2, 'Luis', 'García', 'luis.garcia@mail.com', '23456789', 'pass123'),
(3, 'María', 'López', 'maria.lopez@mail.com', '34567890', 'pass123');

-- Categoría de Directivos
INSERT INTO CategoriasDirectivos (idCatDirectiva, nombre)
VALUES (1, 'Expositor'),
(2, 'Coordinador'),
(3, 'Ayudante');

-- Reuniones
INSERT INTO Reuniones (idReunion, idEvento, idLugar, horarioInicio, horarioFinalizacion) VALUES
(1, 1, 1, '2025-06-22 09:00:00', '2025-06-22 12:00:00'),
(2, 1, 1, '2025-06-29 09:00:00', '2025-06-29 12:00:00'),
(3, 1, 1, '2025-07-06 09:00:00', '2025-07-06 12:00:00');

-- Inscripciones
INSERT INTO Inscripciones (idEvento, idParticipante) VALUES
(1, 1),
(1, 2),
(1, 3);

-- DIRECTIVOS
INSERT INTO Directivos (idReunion, idParticipante, idCatDirectiva) VALUES
(1, 1, 1),
(2, 2, 1),
(3, 3, 1);

