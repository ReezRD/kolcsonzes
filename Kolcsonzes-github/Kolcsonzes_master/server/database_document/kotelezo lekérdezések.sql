select * from loaning;
select * from opus;
select * from specimen;
select * from student;
select * from users;

# get .../student
  SELECT * from student;

# get loaningSpecimen (inner join)
select * from loaning l
  inner join specimen s on s.sId = l.id;

# get opusSpecimen
  select * from opus o 
    inner join specimen s on s.opusid  = o.id;

# get everything

  select o.id, o.writer, o.title, o.year, s.sId, s.opusid, s.price, s.acquisition, l.id, l.specimentid, l.studentid, l.away, l.back, st.id, st.name, st.year  from opus o
    inner join specimen s on s.opusid = o.id
    inner join loaning l on l.specimentid = s.sId
    inner join student st on st.id = l.studentid
    order by st.name;

# get opus by search

  select * from opus
    where writer or title or year == writer or title or year;

# get student by search

select * from student
  where name like '%bor%';

update student set
  name = 'Móra Valaki3',
  year = 11
  where id = 269;

select * from loaning
  where id = 2;

  INSERT specimen
  ( opusid, price, acquisition)
  VALUES
  ( 10, 1125, '2002.09.24');

  update specimen set
  opusid = 10,
  price = 1125,
  acquisition = '2002.09.23'
  where sId = 1104;